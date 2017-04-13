using System;
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

		internal static void SaveToAppFolder(Stream input)
		{
			string path = Parser.ApplicationFolder;

			using (Stream output = File.Create(path))
			{
				input.Seek(0, SeekOrigin.Begin);
				input.CopyTo(output);
			}
		}
	}
}
