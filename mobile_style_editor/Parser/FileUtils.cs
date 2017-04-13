using System;
using System.Collections.Generic;
using System.IO;

namespace mobile_style_editor
{
	public class FileUtils
	{
		public static void OverwriteFileAtPath(string path, string text)
		{
			using (var streamWriter = new StreamWriter(path, false))
			{
				streamWriter.Write(text);
			}
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
			string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			string path = Path.Combine(folder, filename);

			using (Stream output = File.Create(path))
			{
				input.CopyTo(output);
			}

			return new List<string> { filename, folder, path};
		}
	}
}
