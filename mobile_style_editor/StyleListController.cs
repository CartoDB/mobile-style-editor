
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Carto.Ui;
using Xamarin.Forms;

#if __IOS__
using Xamarin.Forms.Platform.iOS;
#elif __ANDROID__
using Xamarin.Forms.Platform.Android;
#elif __UWP__
using Xamarin.Forms.Platform.UWP;
#endif

namespace mobile_style_editor
{
	public class StyleListController : ContentPage
	{
		public StyleView ContentView { get; private set; }

		public StyleListController()
		{
			Title = "CARTO STYLE VIEWER";

			ContentView = new StyleView();
			Content = ContentView;

#if __IOS__
			DriveClientiOS.Instance.Authenticate();
#endif
		}

		bool filesDownloaded;

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			if (!filesDownloaded)
			{
				List<DownloadResult> results = await GetContents();
				ContentView.Templates.ShowSampleStyles(results);
				filesDownloaded = true;
			}

			ShowMyStyles();

			ContentView.AddStyle.Drive.Click += OnDriveButtonClick;
			ContentView.AddStyle.Github.Click += OnGithubButtonClick;

			ContentView.MyStyles.ItemClick += OnStyleClick;
			ContentView.Templates.ItemClick += OnStyleClick;

			ContentView.Popup.Header.BackButton.Click += OnPopupBackButtonClick;
			ContentView.Popup.Header.Select.Click += OnSelectClick;

			ContentView.Popup.FileContent.ItemClick += OnItemClicked;

			HubClient.Instance.FileDownloadStarted += OnGithubFileDownloadComplete;
#if __ANDROID__
			DriveClientDroid.Instance.DownloadStarted += OnDownloadStarted;
			DriveClientDroid.Instance.DownloadComplete += OnFileDownloadComplete;
#elif __IOS__
			DriveClientiOS.Instance.DownloadComplete += OnFileDownloadComplete;
			DriveClientiOS.Instance.ListDownloadComplete += OnListDownloadComplete;
#endif

#if __ANDROID__
			ContentView.ShowMapViews();
#endif
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			ContentView.AddStyle.Drive.Click -= OnDriveButtonClick;
			ContentView.AddStyle.Github.Click -= OnGithubButtonClick;

			ContentView.MyStyles.ItemClick -= OnStyleClick;
			ContentView.Templates.ItemClick -= OnStyleClick;

			ContentView.Popup.Header.BackButton.Click -= OnPopupBackButtonClick;
			ContentView.Popup.Header.Select.Click -= OnSelectClick;

			ContentView.Popup.FileContent.ItemClick -= OnItemClicked;

			HubClient.Instance.FileDownloadStarted -= OnGithubFileDownloadComplete;
#if __ANDROID__
			DriveClientDroid.Instance.DownloadStarted -= OnDownloadStarted;
			DriveClientDroid.Instance.DownloadComplete -= OnFileDownloadComplete;
#elif __IOS__
			DriveClientiOS.Instance.DownloadComplete -= OnFileDownloadComplete;
			DriveClientiOS.Instance.ListDownloadComplete -= OnListDownloadComplete;
#endif
		}

		List<List<GithubFile>> storedContents = new List<List<GithubFile>>();

		void OnPopupBackButtonClick(object sender, EventArgs e)
		{
			if (ContentView.Loader.IsRunning)
			{
				return;
			}

			if (storedContents.Count == 0)
			{
				return;
			}

			if (storedContents.Count == 1)
			{
				ContentView.Popup.Header.BackButton.Disable();
			}

			List<GithubFile> files = storedContents[storedContents.Count - 1];
			ContentView.Popup.Show(files);
			storedContents.Remove(files);

			ContentView.Popup.Header.OnBackPress();
		}

		async void OnSelectClick(object sender, EventArgs e)
		{

			if (ContentView.Popup.GithubFiles == null)
			{
				return;
			}

			Device.BeginInvokeOnMainThread(delegate
			{
				ContentView.Popup.Hide();
				ContentView.ShowLoading();
			});

			List<GithubFile> folder = ContentView.Popup.GithubFiles;
			List<DownloadedGithubFile> files = await HubClient.Instance.DownloadFolder(GithubOwner, GithubRepo, folder);

			Toast.Show("Saving...", ContentView);

			/*
			 * This is where we update pathing -> Shouldn't use repository folder hierarcy
			 * We get the root of the style folder, and use that as the root folder for our folder hierarcy
			 * 
			 * We can safely assume the root folder of the style, not the repository,
			 * contains a config file (currently only supports project.json)
			 * and therefore can remove any other folder hierarchy
			 * 
			 * TODO What if it always isn't like that? && support other types of config files
			 * 
			 */

			DownloadedGithubFile projectFile = files.Find(file => file.IsProjectFile);

			string folderPath = projectFile.FolderPath;
			string[] split = folderPath.Split('/');

			string rootFolder = split[split.Length - 1];
			int length = folderPath.IndexOf(rootFolder, StringComparison.Ordinal);
			string repoRootFolder = folderPath.Substring(0, length);

			foreach (DownloadedGithubFile file in files)
			{
				file.Path = file.Path.Replace(repoRootFolder, "");
				FileUtils.SaveToAppFolder(file.Stream, file.Path, file.Name);
			}

			string zipname = rootFolder + Parser.ZipExtension;
			string source = Path.Combine(Parser.ApplicationFolder, rootFolder);

			Toast.Show("Comperssing...", ContentView);

			string destination = Parser.Compress(source, zipname, MyStyleFolder);
			// Destination contains filename, just remove it
			destination = destination.Replace(zipname, "");

			Toast.Show("Done!", ContentView);

			ShowMyStyles();
		}

		void ShowMyStyles()
		{
			List<string> paths = FileUtils.GetStylesFromFolder(MyStyleFolder);
			List<DownloadResult> data = FileUtils.GetDataFromPaths(paths);

			Device.BeginInvokeOnMainThread(delegate
			{
				ContentView.MyStyles.ShowSampleStyles(data);
			});
		}

		public void OnGithubFileDownloadComplete(object sender, EventArgs e)
		{
			string name = (string)sender;
			Toast.Show("Downloading: " + name, ContentView);
		}

		void OnDownloadStarted(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				ContentView.Popup.Hide();
				ContentView.ShowLoading();
			});
		}

		const string MyStyleFolder = "my-styles";

		void OnFileDownloadComplete(object sender, DownloadEventArgs e)
		{
			FileUtils.SaveToAppFolder(e.Stream, MyStyleFolder, e.Name);

			Device.BeginInvokeOnMainThread(delegate
			{
				ContentView.HideLoading();
			});
			ShowMyStyles();

		}

		const string GithubOwner = "CartoDB";
		const string GithubRepo = "mobile-styles";
		static string BasePath = (GithubRepo + "/").ToUpper();

		async void OnGithubButtonClick(object sender, EventArgs e)
		{
			ContentView.ShowLoading();
			var contents = await HubClient.Instance.GetRepositoryContent(GithubOwner, GithubRepo);
			OnListDownloadComplete(null, new ListDownloadEventArgs { GithubFiles = contents.ToGithubFiles() });

			ContentView.Popup.Header.BackButton.Disable();
			ContentView.Popup.Header.Text = BasePath;
		}

#if __UWP__
		async 
#endif
		void OnDriveButtonClick(object sender, EventArgs e)
		{
#if __ANDROID__
			DriveClientDroid.Instance.Register(Forms.Context);
			DriveClientDroid.Instance.Connect();
#elif __IOS__
			ContentView.ShowLoading();
			DriveClientiOS.Instance.DownloadStyleList();
#elif __UWP__
            ContentView.ShowLoading();
            /*
             * If you crash here, you're probably missing drive_client_ids.json that needs to be bundled as an asset,
             * but because of security reasons, it's not on github.
             * Keys for nutitab@gmail.com are available on Carto's Google Drive under Technology/Product Development/Mobile/keys
             * Simply copy drive_client_ids.json into this project's Assets folder
             * 
             * If you wish to create your ids and refresh tokens,
             * there's a guide under DriveClientiOS
             */
            List<DriveFile> files = await DriveClientUWP.Instance.DownloadFileList();
            OnListDownloadComplete(null, new ListDownloadEventArgs { DriveFiles = files });
#endif
		}

		void OnListDownloadComplete(object sender, ListDownloadEventArgs e)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				if (e.DriveFiles != null)
				{
					ContentView.Popup.Show(e.DriveFiles);
				}
				else if (e.GithubFiles != null)
				{
					ContentView.Popup.Show(e.GithubFiles);
				}

				ContentView.HideLoading();
			});
		}

		async void OnItemClicked(object sender, EventArgs e)
		{
			FileListPopupItem item = (FileListPopupItem)sender;

			if (!item.IsEnabled)
			{
				Alert("This style is not publicly available and cannot be downloaded");
				return;
			}

			Device.BeginInvokeOnMainThread(delegate
			{
				if (item.GithubFile == null)
				{
					// Do not Hide the popup when dealing with github,
					// as a new page should load almost immediately
					// and we need to show content there as well
					ContentView.Popup.Hide();
				}

				ContentView.ShowLoading();
			});

			if (item.Style != null)
			{
				await Navigation.PushAsync(new MainController(item.Style.Path, item.Style.Name));
				Device.BeginInvokeOnMainThread(delegate
				{
					ContentView.HideLoading();
				});
			}
			else if (item.DriveFile != null)
			{
#if __ANDROID__
				/*
				 * Android uses a full-fledged Google Drive component.
				 * No need to manually handle clicks -> Goes straight to FileDownloadComplete()
				 */
#elif __IOS__
				DriveClientiOS.Instance.DownloadStyle(item.DriveFile.Id, item.DriveFile.Name);
#elif __UWP__
                Stream stream = await DriveClientUWP.Instance.DownloadStyle(item.DriveFile.Id, item.DriveFile.Name);

                if (stream == null)
                {
                    Device.BeginInvokeOnMainThread(delegate
                    {
                        ContentView.Popup.Show();
                        ContentView.HideLoading();
                        item.Disable();
                        Alert("This style is not publicly available and cannot be downloaded");
                    });

                } else
                {
                    OnFileDownloadComplete(null, new DownloadEventArgs { Name = item.DriveFile.Name, Stream = stream });
                }
#endif
			}
			else if (item.GithubFile != null)
			{
				if (!item.GithubFile.IsDirectory)
				{
					// Do nothing if it's a file click
					return;
				}
				else
				{
					storedContents.Add(ContentView.Popup.GithubFiles);
					string path = item.GithubFile.Path;
					var contents = await HubClient.Instance.GetRepositoryContent(GithubOwner, GithubRepo, path);
					ContentView.Popup.Show(contents.ToGithubFiles());
					ContentView.HideLoading();

					ContentView.Popup.Header.BackButton.Enable();
					ContentView.Popup.Header.Text = BasePath + path.ToUpper();
				}
			}
		}

		public
#if __UWP__
		async 
#endif
		void Alert(string message)
		{
#if __UWP__
            var dialog = new Windows.UI.Popups.MessageDialog(message);
            await dialog.ShowAsync();
#endif
		}

		void OnStyleClick(object sender, EventArgs e)
		{
			StyleListItem item = (StyleListItem)sender;

			Device.BeginInvokeOnMainThread(async delegate
			{
				await Navigation.PushAsync(new MainController(item.Data.Path, item.Data.Filename));
#if __ANDROID__
				ContentView.HideMapViews();
#endif
			});
		}

		public async System.Threading.Tasks.Task<List<DownloadResult>> GetContents()
		{
			List<Octokit.RepositoryContent> zipfiles = await HubClient.Instance.GetZipFiles("CartoDB", "mobile-sample-styles");

			List<DownloadResult> results = new List<DownloadResult>();

			foreach (var content in zipfiles)
			{
				bool existsLocally = FileUtils.HasLocalCopy(content.Name);

				string path = "";
				string filename = "";

				if (existsLocally)
				{
					Console.WriteLine("Using local file: " + content.Name);

					path = FileUtils.GetLocalPath();
					filename = content.Name;
				}
				else
				{
					Console.WriteLine("Downloading: " + content.Name);

					var file = await HubClient.Instance.DownloadFile(content);
					List<string> data = FileUtils.SaveToAppFolder(file.Stream, file.Name);

					path = data[1];
					filename = data[0];
				}

				results.Add(new DownloadResult { Path = path, Filename = filename });
			}
			
			return results;
		}

		public async Task<List<Octokit.RepositoryContent>> DownloadList()
		{
			return await HubClient.Instance.GetZipFiles("CartoDB", "mobile-sample-styles");
		}
		
		public async Task<DownloadResult> DownloadFile(Octokit.RepositoryContent content)
		{
			var file = await HubClient.Instance.DownloadFile(content);
			List<string> data = FileUtils.SaveToAppFolder(file.Stream, file.Name);
			
			return new DownloadResult { Path = data[1], Filename = data[0] };
		}

	}
}
