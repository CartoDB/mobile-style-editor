using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace mobile_style_editor
{
	public class FileUtils
	{
		public static bool HasLocalCopy(string folder, string name)
		{
			string path = GetLocalPath(folder);
			return File.Exists(Path.Combine(path, name));
		}

		public static string GetLocalPath(string folder = null)
		{
			if (folder != null)
			{
				string path = Path.Combine(Parser.ApplicationFolder, folder);

				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}

				return path;
			}
			
			return Parser.ApplicationFolder;
		}

		public static void OverwriteFileAtPath(string path, string text)
		{
#if __UWP__
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Write))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(text);
                }
            }
#else
			using (var streamWriter = new StreamWriter(path, false))
			{
				streamWriter.Write(text);
			}
#endif
		}

		public static byte[] PathToByteData(string path)
		{
			return File.ReadAllBytes(path);
		}

		/*
		 * Always returns a list of strings where:
		 * 0: filename
		 * 1: folder
		 * 2: combined full path
		 */
		public static List<string> SaveToAppFolder(Stream input, string filename)
		{
			string folder = Parser.ApplicationFolder;

			string path = Path.Combine(folder, filename);

			using (Stream output = File.Create(path))
			{
#if __IOS__
				// Move to 0 position, may not always be the case when dealing with MemoryStreams
				input.Seek(0, SeekOrigin.Begin);
#endif
				input.CopyTo(output);
			}

			return new List<string> { filename, folder, path };
		}

		/*
		* Always returns a list of strings where:
		* 0: filename
		* 1: folder
		* 2: combined full path
		*/
		public static List<string> SaveToAppFolder(Stream input, string folderPath, string filename)
		{
			string baseFolder = Parser.ApplicationFolder;
			string path = Path.Combine(baseFolder, folderPath);

			string folderWithoutFile = path.Replace(filename, "");

			if (!Directory.Exists(folderWithoutFile))
			{
				Directory.CreateDirectory(folderWithoutFile);
			}

			if (!path.Contains(filename))
			{
				/*
				 * Different use-cases. Sometimes folderPath will be full path,
				 * in another method, just the folder name is given as parameters
				 */
				path = Path.Combine(path, filename);
			}

			using (Stream output = File.Create(path))
			{
				try
				{
					input.Seek(0, SeekOrigin.Begin);
				}
				catch (Exception e)
				{
					/*
					 * Seek may or may not be necessary, depending on which type of file we're saving.
					 * Try every time and catch exceptions where it's not supported
					 */
					Console.WriteLine("Exception (" + e + "): " + e.Message);
				}

				input.CopyTo(output);
			}

			return new List<string> { filename, folderWithoutFile, path };
		}

		public static List<string> GetStylesFromFolder(string folder)
		{
			string[] files = { };

			try
			{
				files = Directory.GetFiles(Path.Combine(GetLocalPath(), folder));
			}
			catch(Exception e)
			{
				Console.WriteLine("Message: " + e.Message);
			}

			return files.Where(file => file.Contains(Parser.ZipExtension)).ToList();
		}

		public static List<DownloadResult> GetDataFromPaths(List<string> paths)
		{
			List<DownloadResult> results = new List<DownloadResult>();

			foreach (string path in paths)
			{
				string[] split = path.Split('/');
				string filename = split[split.Length - 1];
				string filepath = path.Replace("/" + filename, "");

				results.Add(new DownloadResult { Filename = filename, Path = filepath });
			}
			return results;
		}
	}
}
