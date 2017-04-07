using System;
using System.IO;

namespace mobile_style_editor
{
	public class FileWriter
	{
		public static void ToPath(string path, string text)
		{
			using (var streamWriter = new StreamWriter(path, false))
			{
				streamWriter.Write(text);
			}

		}
	}
}
