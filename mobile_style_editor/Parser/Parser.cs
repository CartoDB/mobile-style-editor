using System;
using System.Collections.Generic;
using System.IO;
//using System.IO.Compression;
using System.Reflection;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;

namespace mobile_style_editor
{
	public class Parser
	{
		const string Style = "cartodark";

		const string MSSExtension = ".mss";
		const string ZipExtension = ".zip";

		static string FileName { get { return Style + ZipExtension; } }
		static string ApplicationFolder { get { return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); } }

		static string FullFilePath { get { return Path.Combine(ApplicationFolder, FileName); } }

		static string assetPath;

		public static ZipData GetZipData()
		{
			/* TODO
			 * Loading from assets is a temporary solution
			 * Eventually it will be a network query
			 */
			Assembly assembly = Assembly.GetAssembly(typeof(Parser));
			string[] resources = assembly.GetManifestResourceNames();

			foreach (var resource in resources)
			{
				if (resource.Contains(Style))
				{
					assetPath = resource;
				}
			}

			ZipData data = new ZipData();

			using (var output = File.Create(FullFilePath))
			using (var input = assembly.GetManifestResourceStream(assetPath))
			{
				input.CopyTo(output);
			}

			string newPath = ApplicationFolder;

			data.ZipFile = assetPath;

			List<string> paths = Decompress(FullFilePath, newPath);

			data.FilePaths = paths;

			foreach (string path in paths)
			{
				using (var streamReader = new StreamReader(path))
				{
					string content = streamReader.ReadToEnd();
					data.DecompressedFiles.Add(content);
				}
			}

			return data;
		}

		public static List<string> Decompress(string archiveFilenameIn, string outFolder)
		{
			/* NB Need to manually link encoding for iOS:
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

					if (entryFileName.Contains(MSSExtension))
					{
						paths.Add(System.IO.Path.Combine(outFolder, entryFileName));
						Console.WriteLine("Unzipped .mss file: " + entryFileName);
					}

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
