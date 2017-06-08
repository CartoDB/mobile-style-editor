
using System;
using SQLite;

namespace mobile_style_editor
{
    public class RepositoryData
    {
        // Consists of Repository.Id + Branch.Name
        [PrimaryKey]
        public string Id { get; set; }

        public double GithubId { get; set; }

        public string Owner { get; set; }

        public string Name { get; set; }

        public string RepositoryPath { get; set; }

        public string LocalPath { get; set; }

        public string Branch { get; set; }

        public string StyleName { get; set; }

		public string FullLocalPath
		{
			get { return System.IO.Path.Combine(Parser.ApplicationFolder, LocalPath); }
		}

        public void ConstructPrimaryKey()
        {
            Id = GithubId + "-" + RepositoryPath + "-" + Branch;
        }

    }
}
