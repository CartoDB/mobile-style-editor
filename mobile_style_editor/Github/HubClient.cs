using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Octokit;

namespace mobile_style_editor
{
	public class HubClient
	{
		public static readonly HubClient Instance = new HubClient();

		readonly GitHubClient client;

		HubClient()
		{
			client = new GitHubClient(new ProductHeaderValue("com.carto.style.editor"));
		}

		public async Task<User> GetUser(string name)
		{
			return await client.User.Get(name);
		}

		public async Task<IReadOnlyList<Repository>> GetRepositories(string username)
		{
			return await client.Repository.GetAllForUser(username);
		}

		public Task<IReadOnlyList<RepositoryContent>> GetRepositoryContent(string owner, string name)
		{
			return client.Repository.Content.GetAllContents(owner, name);
		}

		public async Task<string> DownloadFile(RepositoryContent content)
		{
			string url = content.DownloadUrl.OriginalString;
			string result;

			using (var client = new HttpClient())
			{
				HttpResponseMessage response = await client.GetAsync(url);
				result = await response.Content.ReadAsStringAsync();
			}

			return result;
		}
	
		public async void UploadFile(Repository repository, RepositoryContent file, string content)
		{
			long id = repository.Id;
			string path = ".gitignore";//file.DownloadUrl.AbsolutePath;
			//path = path.Replace(repository.Owner.Login 
			var request = new UpdateFileRequest("test upload from octokit api", content, file.Sha);

			string url = file.DownloadUrl.AbsolutePath;
			string[] split = url.Split('/');

			string owner = split[1];
			string name = split[2];
			string branch = split[3];
			path = split[4];

			request = new UpdateFileRequest("test upload from octokit api", content, file.Sha, branch);
			try
			{
				await client.Repository.Content.UpdateFile(owner, name, path, request);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			 
				try
			{
				await client.Repository.Content.UpdateFile(id, path, request);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}
