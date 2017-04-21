
using System;
using System.Collections.Generic;

namespace mobile_style_editor
{
	public class ListDownloadEventArgs : EventArgs
	{
		public List<DriveFile> Items { get; set; }
	}
}
