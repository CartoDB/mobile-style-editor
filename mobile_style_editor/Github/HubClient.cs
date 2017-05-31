using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Carto.Core;
using Octokit;

namespace mobile_style_editor
{
	public class HubClient
	{
		public static readonly HubClient Instance = new HubClient();

		public EventHandler<EventArgs> FileDownloadStarted;

		/*
		 * Current flow:
		 * 
		 * (1) Registered OAuth Application on github.com: Carto Style Editor (creates ClientId and ClientSecret),
		 * that we use to open the Webview at the correct (login) url, from PrepareAuthentication()
		 * 
		 * (2) If login is successful (two-factor authentication support included),
		 * we are redirected (currently https://www.carto.com)
		 * with Login Code as a parameter of the url (?=<code>), that we retrieve from the Webview
		 * 
		 * (3) The code, as well as the ClientId and ClientSecret are required to get Access Token (CreateAccessToken())
		 * 
		 * (4) When the token is created, we use it to Authenticate() and store it as a preference (cf. LocalStorage.cs).
		 * 
		 * This process is only required once, as later we retrieve the stored access token,
		 * use that to Authenticate() and we can start retrieving repository content
		 * 
		 * TODO Ask if a user would like their access token to be stored locally,
		 * it's not nice (and probably illegal) to store and use personal information without their consent
		 * 
		 * NOTES:
		 * 
         * Because of the low rate limit for un-authenticated users,
		 * authentication is necessary even when accessing public repositories:
		 * https://developer.github.com/changes/2012-10-14-rate-limit-changes/
		 * 
		 * This entire complicated login process is required only so each user could authenticate themself,
		 * for inhouse use-cases we could simply create one access token in one account and use that,
		 * (https://github.com/settings/tokens) instead of this entire process
		 * 
		 */

		GitHubClient client;

		public bool IsAuthenticated
		{
			get
			{
				return client.Credentials.AuthenticationType == AuthenticationType.Oauth && client.Credentials.Password != null;
			}
		}

		HubClient()
		{
			Initialize();
		}

        void Initialize()
        {
            client = new GitHubClient(new ProductHeaderValue("com.carto.style.editor"));
        }

        public async Task<User> GetCurrentUser()
        {
            return await client.User.Current();
        }

        public async Task<Stream> GetUserAvatar(string url)
        {
			using (var client = new HttpClient())
			{
				HttpResponseMessage response = await client.GetAsync(url);
				return await response.Content.ReadAsStreamAsync();
			}
        }

        public void LogOut()
        {
            Initialize();
        }

		public const int PageSize = 50;
		const int PageCount = 1;

		ApiOptions GetOptions(int page)
		{
			var options = new ApiOptions();

			options.PageSize = PageSize;
			options.PageCount = PageCount;

			options.StartPage = page;

			return options;
		}

		public void Authenticate(string token)
		{
			client.Credentials = new Credentials(token);
		}

		public GithubAuthenticationData PrepareAuthention()
		{
			Dictionary<string, string> dict = GetCredentials();

			string id = dict["client_id"];
			string secret = dict["client_secret"];

			var request = new OauthLoginRequest(id);
			request.Scopes.Add("repo");

			var url = client.Oauth.GetGitHubLoginUrl(request);

			return new GithubAuthenticationData
			{
				Id = id,
				Secret = secret,
				Url = url.AbsoluteUri
			};
		}

		public async Task<string> CreateAccessToken(string id, string secret, string code)
		{
			var request = new OauthTokenRequest(id, secret, code);

			OauthToken token = await client.Oauth.CreateAccessToken(request);
			return token.AccessToken;
		}

		public async Task<IReadOnlyList<Repository>> GetRepositories(int page = -1)
		{
			/*
			 * This method only works when user has been Authenticate()-d,
			 * else there is no "Current" user
			 */
			if (page != -1)
			{
				return await client.Repository.GetAllForCurrent(GetOptions(page));
			}

			return await client.Repository.GetAllForCurrent();
		}

		public async Task<IReadOnlyList<RepositoryContent>> GetRepositoryContent(string owner, string name, string path = null)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(path))
				{
					return await client.Repository.Content.GetAllContents(owner, name);
				}

				return await client.Repository.Content.GetAllContents(owner, name, path);
			}
			catch (NotFoundException)
			{
				// For some reasons, Octokit throws an exception when a repository is completely empty.
				// In that case, return an empty list
				return new System.Collections.ObjectModel.ReadOnlyCollection<RepositoryContent>(new List<RepositoryContent>());
			}
		}

		public async Task<List<RepositoryContent>> GetZipFiles(string owner, string name, string path = null)
		{
			IReadOnlyList<RepositoryContent> contents;

			if (path != null)
			{
				contents = await client.Repository.Content.GetAllContents(owner, name, path);
			}
			else
			{
				contents = await client.Repository.Content.GetAllContents(owner, name);
			}

			List<RepositoryContent> zipfiles = new List<RepositoryContent>();

			foreach (var content in contents)
			{
				if (content.Name.Contains(Parser.ZipExtension))
				{
					zipfiles.Add(content);
				}
			}

			return zipfiles;
		}

		public async Task<DownloadedGithubFile> DownloadFile(RepositoryContent content)
		{
			string name = content.Name;
			string url = content.DownloadUrl.OriginalString;
			string path = content.Path;
			return await DownloadFile(name, url, path);
		}

		public async Task<DownloadedGithubFile> DownloadFile(GithubFile content)
		{
			string name = content.Name;
			string url = content.DownloadUrl;
			string path = content.Path;
			return await DownloadFile(name, url, path);
		}

		public async Task<DownloadedGithubFile> DownloadFile(string name, string url, string path)
		{
			DownloadedGithubFile result = new DownloadedGithubFile();;

			using (var client = new HttpClient())
			{
				HttpResponseMessage response = await client.GetAsync(url);

				result.Name = name;
				result.Path = path;
				result.Stream = await response.Content.ReadAsStreamAsync();
			}

			return result;
		}

		public async Task<List<DownloadedGithubFile>> DownloadFolder(string owner, string repoName, List<GithubFile> folder)
		{
			List<DownloadedGithubFile> files = new List<DownloadedGithubFile>();

			foreach (GithubFile file in folder)
			{
				if (file.IsDirectory)
				{
					string path = file.Path;
					var items = await GetRepositoryContent(owner, repoName, path);
					List<GithubFile> innerFolder = items.ToGithubFiles();
					List<DownloadedGithubFile> inner = await DownloadFolder(owner, repoName, innerFolder);
					files.AddRange(inner);
				}
				else
				{
					if (FileDownloadStarted != null)
					{
						FileDownloadStarted(file.Name, EventArgs.Empty);
					}

					Console.WriteLine("Downloading: " + file.Name + " (" + file.DownloadUrl + ")");
					DownloadedGithubFile downloaded = await DownloadFile(file.Name, file.DownloadUrl, file.Path);

					files.Add(downloaded);

				}
			}

			return files;
		}

		public async Task<bool> UpdateFile(Repository repository, RepositoryContent file, string content)
		{
			string url = file.DownloadUrl.AbsolutePath;
			string[] split = url.Split('/');

			/*
			 * TODO Splitting like this will not always yield positive results.
			 * e.g. it'll be longer if the file is in a subfolder
			 */
			string owner = split[1];
			string name = split[2];
			string branch = split[3];
			string path = split[4];

			// Both seem to work now, since the branch has been specified
			var request = new UpdateFileRequest("test upload from octokit api", content, file.Sha, branch);

			try
			{
				await client.Repository.Content.UpdateFile(owner, name, path, request);
				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return false;
			}
		}

		public Dictionary<string, string> GetCredentials()
		{
			var dictionary = new Dictionary<string, string>();
#if __UWP__
            Assembly assembly = typeof(Parser).GetTypeInfo().Assembly;
#else
			Assembly assembly = Assembly.GetAssembly(typeof(HubClient));
#endif
			string[] resources = assembly.GetManifestResourceNames();

			string name = "github_info.json";
			string path = null;

			foreach (var resource in resources)
			{
				if (resource.Contains(name) && !resource.Contains("with-params"))
				{
					path = resource;
				}
			}

			using (var stream = assembly.GetManifestResourceStream(path))
			{
				stream.Position = 0;
				using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
				{
					string result = reader.ReadToEnd();

					Variant variant = Variant.FromString(result);
					dictionary.Add("username", variant.GetObjectElement("username").String);
					dictionary.Add("pa_token", variant.GetObjectElement("pa_token").String);

					dictionary.Add("client_id", variant.GetObjectElement("client_id").String);
					dictionary.Add("client_secret", variant.GetObjectElement("client_secret").String);
				}
			}

			return dictionary;
		}

	}
}
