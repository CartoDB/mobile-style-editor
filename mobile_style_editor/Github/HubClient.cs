using System;
using System.Collections.Generic;
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
	}
}
