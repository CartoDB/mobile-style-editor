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

		static string Path;

		public Parser()
		{
			Assembly assembly = Assembly.GetAssembly(typeof(Parser));
			string[] resources = assembly.GetManifestResourceNames();

			foreach (var resource in resources)
			{
				if (resource.Contains(Style))
				{
					Path = resource;
				}
			}

			if (Path == null)
			{
				// TODO Error handling
				return;
			}

			string fileName = Style + ZipExtension;
			string applicationFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			string appFolder = System.IO.Path.Combine(applicationFolder, fileName);

			using (var output = File.Create(appFolder))
			using (var input = assembly.GetManifestResourceStream(Path))
			{
				input.CopyTo(output);
			}

			string newPath = applicationFolder;

			List<string> paths = Decompress(appFolder, newPath);

			foreach (string path in paths)
			{
				Console.WriteLine(".mss file path: " + path);
			}

			using (var streamReader = new StreamReader(paths[0]))
			{
				string content = streamReader.ReadToEnd();
				System.Diagnostics.Debug.WriteLine(content);
			}
		}

		public List<string> Decompress(string archiveFilenameIn, string outFolder)
		{
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
						continue;           // Ignore directories
					}

					string entryFileName = zipEntry.Name;

					if (entryFileName.Contains(MSSExtension))
					{
						paths.Add(System.IO.Path.Combine(outFolder, entryFileName));
						Console.WriteLine("Unzipped .mss file: " + entryFileName);
					}

					// to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
					// Optionally match entrynames against a selection list here to skip as desired.
					// The unpacked length is available in the zipEntry.Size property.

					byte[] buffer = new byte[4096];     // 4K is optimum
					Stream zipStream = zf.GetInputStream(zipEntry);

					// Manipulate the output filename here as desired.
					string fullZipToPath = System.IO.Path.Combine(outFolder, entryFileName);
					string directoryName = System.IO.Path.GetDirectoryName(fullZipToPath);
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
