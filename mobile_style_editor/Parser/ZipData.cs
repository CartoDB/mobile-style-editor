
using System;
using System.Collections.Generic;

namespace mobile_style_editor
{
	public class ZipData
	{
		public string Filename { get; set; }

		public string DecompressedPath { get; set; }

		public List<string> AllFilePaths { get; set; }

		public List<string> DecompressedFiles { get; set; }

		public List<string> StyleFilePaths { get; set; }

		public List<string> StyleFileNames { get; set; }

        public List<string> ChangeList { get; set; }

        public int ProjectFileIndex
        {
            get
            { 
                string file = StyleFileNames.Find(f => f.Equals(Parser.ProjectFile));
                return StyleFileNames.IndexOf(file);
            }
        }
		
        public ZipData()
		{
			DecompressedFiles = new List<string>();

			StyleFilePaths = new List<string>();

			StyleFileNames = new List<string>();

            ChangeList = new List<string>();
		}
	}
}
