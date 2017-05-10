using System;
namespace mobile_style_editor
{
	public class DownloadResult
	{
		public string Path { get; set; }

		public string Filename { get; set; }

		public string CleanName { get { return Filename.Replace(Parser.ZipExtension, ""); } }

		public byte[] Data
		{
			get
			{
				return FileUtils.PathToByteData(System.IO.Path.Combine(Path, Filename));
			}
		}

	}
}
