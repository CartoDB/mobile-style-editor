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
						if (path.Contains("__MACOSX"))
						{
							/* 
							 * OSX Creates a __MAXOSX folder every time you zip an archive:
							 * 
                             * The Mac OS X has a "forked" filesystem called HFS+ - in particular, 
                             * many files have a "resource fork" in addition to the "data fork" 
                             * (the more common place where data is stored).  
                             * The __MACOSX folders in a zip file allow the zipped file to 
                             * retain the additional forks like the resource fork.
							 * 
							 * However, in our case, we must ignore that folder since it contains metadata
							 * that cannot be parsed into json and causes the application to crash
							 * 
							 * Ideally, if zipping on osx, you should do it without the rather pointless metadata,
							 * as it just increases the size of the file anyway.
							 * You should use a third-party service like Keka (http://www.kekaosx.com/en/) tha
							 * 
							 */
							continue;
						}

						string content = streamReader.ReadToEnd();
						Variant json = Variant.FromString(content);
						Variant styles = json.GetObjectElement("styles");

						for (int i = 0; i < styles.ArraySize; i++)
						{
							string styleFileName = styles.GetArrayElement(i).String;

							foreach (string mssPath in paths)
							{
								if (mssPath.Contains("__MACOSX"))
								{
									/* cf. explanation above */
									continue;
								}

								if (mssPath.Contains(styleFileName))
								{
#if __UWP__
                                        using (var stream2 = new FileStream(mssPath, FileMode.Open, FileAccess.Read))
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

		public static string Compress(string source, string newFilename, string destinationFolder = null)
		{
			FastZip instance = new FastZip();
			instance.CreateEmptyDirectories = true;

			string folder = ApplicationFolder;

			if (destinationFolder != null)
			{
				folder = Path.Combine(folder, destinationFolder);
				if (!Directory.Exists(folder))
				{
					Directory.CreateDirectory(folder);
				}
			}

			string destination = Path.Combine(folder, newFilename);

#if __UWP__
            // Need to Set Virtual File System on UWP
            // More information at: https://github.com/ygrenier/SharpZipLib.Portable#virtual-file-system
            ICSharpCode.SharpZipLib.VFS.SetCurrent(new UWPVFS());
#endif
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
