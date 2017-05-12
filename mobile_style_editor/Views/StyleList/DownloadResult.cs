using System;
namespace mobile_style_editor
{
	public class DownloadResult
	{
		public string Path { get; set; }

		public string Filename { get; set; }

		public string CleanName { get { return ToCleanName(Filename); } }

		public int Index { get; set; }

		public byte[] Data
		{
			get
			{
				return FileUtils.PathToByteData(System.IO.Path.Combine(Path, Filename));
			}
		}

		public static string ToCleanName(string name)
		{
			return name.Replace(Parser.ZipExtension, "").ToUpper();
		}
	}
}
