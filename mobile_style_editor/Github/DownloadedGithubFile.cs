using System;
using System.IO;
using System.Net.Http;
using Octokit;

namespace mobile_style_editor
{
	public class DownloadedGithubFile
	{
		public string Name { get; set; }

		public string Path { get; set; }

		public Stream Stream { get; set; }
	}
}
