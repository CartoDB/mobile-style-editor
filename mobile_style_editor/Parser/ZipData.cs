
using System;
using System.Collections.Generic;

namespace mobile_style_editor
{
	public class ZipData
	{
		public string AssetZipFile { get; set; }

		public string FolderPath { get; set; }

		public List<string> DecompressedFiles { get; set; }

		public List<string> FilePaths { get; set; }

		public List<string> FileNames { 
			get
			{
				List<string> names = new List<string>();

				foreach (string path in FilePaths)
				{
					string[] split = path.Split('/');
					string name = split[split.Length - 1];
					names.Add(name);	
				}

				return names;
			}
		}

		public ZipData()
		{
			DecompressedFiles = new List<string>();
		}
	}
}
