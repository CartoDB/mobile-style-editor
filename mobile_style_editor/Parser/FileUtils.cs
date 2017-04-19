using System;
using System.Collections.Generic;
using System.IO;

namespace mobile_style_editor
{
	public class FileUtils
	{
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
#if __ANDROID__
				// Do nothing, as it's already a FileInputStream
#else
				// Move to 0 position, may not always be the case when dealing with MemoryStreams
				input.Seek(0, SeekOrigin.Begin);
#endif
				input.CopyTo(output);
			}

			return new List<string> { filename, folder, path };
		}
	}
}
