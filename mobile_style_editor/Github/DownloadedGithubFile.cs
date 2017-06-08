using System;
using System.IO;
using System.Net.Http;
using Octokit;

namespace mobile_style_editor
{
	public class DownloadedGithubFile
	{
        public bool IsProjectFile { get { return Path.Contains(Parser.ProjectFile); } }

		public string Name { get; set; }

		public string FolderPath
		{
			get
			{
				string folderPath = Path.Replace(Name, "");
				// Remove trailing '/'
				return folderPath.Substring(0, folderPath.Length - 1);
			}
		}

		public string Path { get; set; }

		public Stream Stream { get; set; }
	}
}
