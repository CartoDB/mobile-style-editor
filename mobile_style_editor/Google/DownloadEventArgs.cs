
using System;
using System.IO;

namespace mobile_style_editor
{
	public class DownloadEventArgs : EventArgs
	{
		public Stream Stream { get; set; }
	}
}
