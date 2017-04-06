
using System;
using System.Collections.Generic;

namespace mobile_style_editor
{
	public class ZipData
	{
		public string ZipFile { get; set; }

		public List<string> DecompressedFiles { get; set; }

		public ZipData()
		{
			DecompressedFiles = new List<string>();
		}
	}
}
