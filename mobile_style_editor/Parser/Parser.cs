using System;
using System.Collections.Generic;
using System.IO;
using Carto.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace mobile_style_editor
{
	public class Parser
	{
		const string ProjectFile = "project.json";

		const string MSSExtension = ".mss";
		public const string ZipExtension = ".zip";

		public static string ApplicationFolder {
            get
            {
#if __UWP__
                return Windows.Storage.ApplicationData.Current.LocalFolder.Path;
#else
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
#endif
            }
        }

		public static string LocalStyleLocation { get { return Path.Combine(ApplicationFolder, "local-styles"); } }

        public static ZipData GetZipData(string folder, string filename)
        {
            ZipData data = new ZipData();

            string fullPath = Path.Combine(folder, filename);
            string newFolder = filename.Replace(ZipExtension, "");
            string decompressedPath = Path.Combine(folder, newFolder);

            List<string> paths = Decompress(fullPath, decompressedPath);

            data.Filename = filename;
            data.DecompressedPath = decompressedPath;
            data.AllFilePaths = paths;

            /*
			 * The Zip itself can contain extra .mss files and presents them in the wrong order.
			 * Read the necessary .mss files and their order from project.json
			 */
            foreach (string path in paths)
            {
                if (path.Contains(ProjectFile))
                {
#if __UWP__
                    using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        using (var streamReader = new StreamReader(stream))
#else
                        using (var streamReader = new StreamReader(path))
#endif
                        {
                            string content = streamReader.ReadToEnd();
                            Variant json = Variant.FromString(content);
                            Variant styles = json.GetObjectElement("styles");

                            for (int i = 0; i < styles.ArraySize; i++)
                            {
                                string styleFileName = styles.GetArrayElement(i).String;

                                foreach (string mssPath in paths)
                                {
                                    if (mssPath.Contains(styleFileName))
                                    {
#if __UWP__
                                        using (var stream2 = new FileStream(path, FileMode.Open, FileAccess.Read))
                                        {
                                            using (var mssReader = new StreamReader(stream2))
#else
											using (var mssReader = new StreamReader(mssPath))
#endif
                                            {
                                                string styleContent = mssReader.ReadToEnd();
                                                data.DecompressedFiles.Add(styleContent);
                                            }

                                            data.StyleFileNames.Add(styleFileName);
                                            data.StyleFilePaths.Add(mssPath);
#if __UWP__
                                        }
#endif
                                    }
                                }
                            }
                        }
#if __UWP__
                    }
#endif
                }
            }

            return data;
        }

		public static string Compress(string source, string newFilename)
		{
			FastZip instance = new FastZip();
			instance.CreateEmptyDirectories = true;

			string destination = Path.Combine(ApplicationFolder, newFilename);

			instance.CreateZip(destination, source, true, "");

			return destination;
		}

		public static List<string> Decompress(string archiveFilenameIn, string outFolder)
		{
			/* 
             * NB Need to manually link encoding for iOS:
			 * Options -> iOS Build -> Additional mtouch aruments: -i18n=west
			 * (http://stackoverflow.com/questions/4600923/monotouch-icsharpcode-sharpziplib-giving-an-error)
			 */

			List<string> paths = new List<string>();

			ZipFile zf = null;
			try
			{
				FileStream fs = File.OpenRead(archiveFilenameIn);

				zf = new ZipFile(fs);

				foreach (ZipEntry zipEntry in zf)
				{
					if (!zipEntry.IsFile)
					{
						// Ignore directories
						continue;
					}

					string entryFileName = zipEntry.Name;

					paths.Add(Path.Combine(outFolder, entryFileName));
				
					// To remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
					// Optionally, match entrynames against a selection list here to skip as desired.
					// The unpacked length is available in the zipEntry.Size property.

					byte[] buffer = new byte[4096];     // 4K is optimum
					Stream zipStream = zf.GetInputStream(zipEntry);

					// Manipulate the output filename here as desired.
					string fullZipToPath = Path.Combine(outFolder, entryFileName);
					string directoryName = Path.GetDirectoryName(fullZipToPath);

					if (directoryName.Length > 0)
					{
						Directory.CreateDirectory(directoryName);
					}

					// Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
					// of the file, but does not waste memory.
					// The "using" will close the stream even if an exception occurs.
					using (FileStream streamWriter = File.Create(fullZipToPath))
					{
						zipStream.Copy(streamWriter, buffer);
					}
				}
			}
			finally
			{
				if (zf != null)
				{
					zf.IsStreamOwner = true; // Makes close also shut the underlying stream
					zf.Close(); // Ensure we release resources
				}
			}

			return paths;
		}

	}
}
