
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
    public class StyleListController : BaseController
	{
		public StyleListView ContentView { get; private set; }

		public StyleListController()
		{
			NavigationPage.SetHasNavigationBar(this, false);

			ContentView = new StyleListView();
			Content = ContentView;

            ContentView.NavigationBar.IsBackButtonVisible = false;
			ContentView.NavigationBar.Title.Text = "CARTO STYLE EDITOR";

#if __IOS__
			DriveClientiOS.Instance.Authenticate();
#endif
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			
			if (LocalStorage.Instance.HasAccessToken)
			{
				string token = LocalStorage.Instance.AccessToken;
				HubClient.Instance.Authenticate(token);
				PopulateTemplateList();
			}
			else
			{
				InitializeAuthentication();
			}

			ContentView.AddStyle.Drive.Click += OnDriveButtonClick;
			ContentView.AddStyle.Github.Click += OnGithubButtonClick;

			ContentView.MyStyles.ItemClick += OnStyleClick;
			ContentView.Templates.ItemClick += OnStyleClick;

            ContentView.Templates.RefreshButton.Click += OnTemplateRefreshClick;

			ContentView.Tabs.TabClicked += OnTabClick;

			ContentView.Webview.Authenticated += OnCodeReceived;

			ContentView.FileList.Header.BackButton.Click += OnPopupBackButtonClick;
			ContentView.FileList.Header.Select.Click += OnSelectClick;

			ContentView.FileList.FileContent.ItemClick += OnItemClicked;
			ContentView.FileList.Pages.PageClicked += OnPageClick;

            ContentView.SettingsButton.Click += OnSettingsClick;
            ContentView.Settings.SettingsContent.UserInfo.LogoutButton.Click += OnLogoutButtonClicked;

			HubClient.Instance.FileDownloadStarted += OnGithubFileDownloadStarted;
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
			ShowMyStyles();
        }

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			ContentView.AddStyle.Drive.Click -= OnDriveButtonClick;
			ContentView.AddStyle.Github.Click -= OnGithubButtonClick;

			ContentView.MyStyles.ItemClick -= OnStyleClick;
			ContentView.Templates.ItemClick -= OnStyleClick;

			ContentView.Templates.RefreshButton.Click -= OnTemplateRefreshClick;

			ContentView.Tabs.TabClicked += OnTabClick;

			ContentView.Webview.Authenticated -= OnCodeReceived;

			ContentView.FileList.Header.BackButton.Click -= OnPopupBackButtonClick;
			ContentView.FileList.Header.Select.Click -= OnSelectClick;

			ContentView.FileList.FileContent.ItemClick -= OnItemClicked;
			ContentView.FileList.Pages.PageClicked -= OnPageClick;

			ContentView.SettingsButton.Click -= OnSettingsClick;
            ContentView.Settings.SettingsContent.UserInfo.LogoutButton.Click -= OnLogoutButtonClicked;

			HubClient.Instance.FileDownloadStarted -= OnGithubFileDownloadStarted;
#if __ANDROID__
            DriveClientDroid.Instance.DownloadStarted -= OnDownloadStarted;
            DriveClientDroid.Instance.DownloadComplete -= OnFileDownloadComplete;
#elif __IOS__
			DriveClientiOS.Instance.DownloadComplete -= OnFileDownloadComplete;
			DriveClientiOS.Instance.ListDownloadComplete -= OnListDownloadComplete;
#endif
		}

        async void OnSettingsClick(object sender, EventArgs e)
        {
            bool showing = ContentView.Settings.Toggle();

            if (showing)
            {
                ContentView.Settings.SettingsContent.ShowLoading();

                Octokit.User user = await HubClient.Instance.GetCurrentUser();
                ContentView.Settings.SettingsContent.UserInfo.Update(user);

                ContentView.Settings.SettingsContent.HideLoading();

                Stream stream = await HubClient.Instance.GetUserAvatar(user.AvatarUrl);
                ContentView.Settings.SettingsContent.UserInfo.Update(stream);
            }
        }

        void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            string message = "Are you sure you wish to log out from Github?";
            Alert("", message, null, delegate
            {
                HubClient.Instance.LogOut();
                LocalStorage.Instance.DeleteToken();
            });

        }

		List<Octokit.RepositoryContent> contents;
        bool FilesDownloaded { get { return contents != null; } }

        async Task<bool> PopulateTemplateList(bool checkLocal = true)
		{
			if (!FilesDownloaded)
			{
				/*
				 * TODO
				 * The following logic relies on the fact that it is an ordered list.
				 * Will render the wrong map if the order has somehow changed
				 * 
				 */
                contents = await DownloadList();
				ContentView.Templates.RenderList(contents);
			}

			int index = 0;

			foreach (var content in contents)
			{
                DownloadResult result = await DownloadFile(content, checkLocal);
				ContentView.Templates.RenderMap(result, index);
				index++;
			}

			return false;
		}

		void InitializeAuthentication()
		{
			GithubAuthenticationData data = HubClient.Instance.PrepareAuthention();
			ContentView.OpenWebviewPopup(data);
		}
                  
        void OnTemplateRefreshClick(object sender, EventArgs e)
        {
            contents = null;
            PopulateTemplateList(false);
        }

		public void OnPageClick(object sender, EventArgs e)
		{
			PageView page = (PageView)sender;
			ContentView.FileList.Show(page.GithubFiles);
		}

		public async void OnCodeReceived(object sender, AuthenticationEventArgs e)
		{
			if (e.IsOk)
			{
				Console.WriteLine("Code: " + e.Code);
				ContentView.Webview.Hide();
				string token = await HubClient.Instance.CreateAccessToken(e.Id, e.Secret, e.Code);
				Console.WriteLine("Token: " + token);

                string message = "Would you like to store your access token so you don't have to log in again?";
                Alert("", message, null, delegate {
                    LocalStorage.Instance.AccessToken = token;
                });
				
				HubClient.Instance.Authenticate(token);
				PopulateTemplateList();

				if (ClickedGithubButton)
				{
					/*
					 * User clicked the button unauthenticated,
					 * went through the whole authentication process,
					 * now repeat step 1 as authenticated user
					 */
					OnGithubButtonClick(null, null);
					ClickedGithubButton = false;
				}
			}
		}

		public void OnTabClick(object sender, EventArgs e)
		{
			ContentView.ScrollTo((StyleTab)sender);
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
				ContentView.FileList.Header.BackButton.Disable();
				ContentView.FileList.Pages.Show();
			}

			List<GithubFile> files = storedContents[storedContents.Count - 1];
			ContentView.FileList.Show(files);
			storedContents.Remove(files);

			ContentView.FileList.Header.OnBackPress();
		}

		async void OnSelectClick(object sender, EventArgs e)
		{
			if (ContentView.FileList.GithubFiles == null)
			{
				return;
			}

			Device.BeginInvokeOnMainThread(delegate
			{
				ContentView.FileList.Hide();
				ContentView.ShowLoading();
			});

			List<GithubFile> folder = ContentView.FileList.GithubFiles;
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

			Device.BeginInvokeOnMainThread(delegate
			{
				ContentView.HideLoading();
			});

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

		public void OnGithubFileDownloadStarted(object sender, EventArgs e)
		{
			string name = (string)sender;
			Toast.Show("Downloading: " + name, ContentView);
		}

		void OnDownloadStarted(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				ContentView.FileList.Hide();
				ContentView.ShowLoading();
			});
		}

		const string MyStyleFolder = "my-styles";
		const string TemplateFolder = "template-styles";

		void OnFileDownloadComplete(object sender, DownloadEventArgs e)
		{
			FileUtils.SaveToAppFolder(e.Stream, MyStyleFolder, e.Name);

			Device.BeginInvokeOnMainThread(delegate
			{
				ContentView.HideLoading();
			});

			ShowMyStyles();
		}

		string GithubOwner = "";
		string GithubRepo = "";
		string GithubPath = "";
		string BasePath { get { return (GithubRepo + "/").ToUpper(); } }

		bool ClickedGithubButton;

		int counter = 1;

		async void DownloadRepositories()
		{
			if (counter == 1)
			{
				ContentView.ShowLoading();
			}

			var contents = await HubClient.Instance.GetRepositories(counter);
			ContentView.FileList.Pages.AddPage(contents.ToGithubFiles(), counter);

			if (counter == 1)
			{
				OnListDownloadComplete(null, new ListDownloadEventArgs { GithubFiles = contents.ToGithubFiles() });
				ContentView.FileList.Header.Text = BasePath;
				ContentView.FileList.Pages.HighlightFirst();
			}

			if (contents.Count == HubClient.PageSize)
			{
				counter++;
				DownloadRepositories();
			}
			else
			{
				counter = 1;
			}
		}

		void OnGithubButtonClick(object sender, EventArgs e)
		{
			if (ContentView.Loader != null && ContentView.Loader.IsRunning)
			{
				return;
			}
 			
			if (HubClient.Instance.IsAuthenticated)
			{
				DownloadRepositories();
			}
			else
			{
				ClickedGithubButton = true;
				InitializeAuthentication();
			}
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
					ContentView.FileList.Show(e.DriveFiles);
				}
				else if (e.GithubFiles != null)
				{
					ContentView.FileList.Show(e.GithubFiles);
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
					ContentView.FileList.Hide();
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
				/*
				 * Ignore file clicks
				 */
				if (item.GithubFile.IsDirectory)
				{
					if (item.GithubFile.IsRepository)
					{
						GithubOwner = item.GithubFile.Owner;
						GithubRepo = item.GithubFile.Name;
						ContentView.FileList.Pages.Hide();
					}

					GithubPath = item.GithubFile.Path;
					await LoadGithubContents();
				}
			}
		}

		async Task<bool> LoadGithubContents()
		{
			storedContents.Add(ContentView.FileList.GithubFiles);

			if (GithubPath == null)
			{
				// Path will be null if we're dealing with a repository
				GithubPath = "";
			}
		
 			var contents = await HubClient.Instance.GetRepositoryContent(GithubOwner, GithubRepo, GithubPath);
			ContentView.FileList.Show(contents.ToGithubFiles());
			ContentView.HideLoading();

			Device.BeginInvokeOnMainThread(delegate
			{
				ContentView.FileList.Header.BackButton.Enable();
				ContentView.FileList.Header.Text = BasePath + GithubPath.ToUpper();
			});

			return true;
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

		public async Task<List<Octokit.RepositoryContent>> DownloadList()
		{
			return await HubClient.Instance.GetZipFiles("CartoDB", "mobile-sample-styles");
		}

        public async Task<DownloadResult> DownloadFile(Octokit.RepositoryContent content, bool checkLocal = true)
        {
            bool existsLocally = true;

            if (checkLocal)
            {
                existsLocally = FileUtils.HasLocalCopy(TemplateFolder, content.Name);
            }
            else
            {
                existsLocally = false;
            }

            string path;
            string filename;

            if (!existsLocally)
            {
                var file = await HubClient.Instance.DownloadFile(content);
                List<string> data = FileUtils.SaveToAppFolder(file.Stream, TemplateFolder, file.Name);

                path = data[1];
                filename = data[0];
            }
            else
            {
                path = FileUtils.GetLocalPath(TemplateFolder);
                filename = content.Name;
            }

            return new DownloadResult { Path = path, Filename = filename };
        }

	}
}
