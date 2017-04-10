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
	}
}
