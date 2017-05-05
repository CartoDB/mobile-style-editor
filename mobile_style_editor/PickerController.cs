using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Octokit;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class PickerController : ContentPage
	{
		public PickerView ContentView { get; private set; }

		public PickerController()
		{
			ContentView = new PickerView();
			Content = ContentView;

#if __IOS__
			DriveClientiOS.Instance.Authenticate();
#endif
			Title = "CARTO STYLE EDITOR";
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			ContentView.Github.Click += OnGithubButtonClick;
			ContentView.Drive.Click += OnDriveButtonClick;
			ContentView.Database.Click += OnDatabaseButtonClick;

			ContentView.Popup.BackButton.Click += OnPopupBackButtonClick;
			ContentView.Popup.Select.Click += OnSelectClick;

			ContentView.Popup.FileContent.ItemClick += OnItemClicked;

			HubClient.Instance.FileDownloadStarted += OnGithubFileDownloadComplete;
#if __ANDROID__
			DriveClientDroid.Instance.DownloadStarted += OnDownloadStarted;
			DriveClientDroid.Instance.DownloadComplete += OnFileDownloadComplete;
#elif __IOS__
			DriveClientiOS.Instance.DownloadComplete += OnFileDownloadComplete;
			DriveClientiOS.Instance.ListDownloadComplete += OnListDownloadComplete;
#endif
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			ContentView.Github.Click -= OnGithubButtonClick;
			ContentView.Drive.Click -= OnDriveButtonClick;
			ContentView.Database.Click -= OnDatabaseButtonClick;

			ContentView.Popup.BackButton.Click -= OnPopupBackButtonClick;
			ContentView.Popup.Select.Click -= OnSelectClick;

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

			List<GithubFile> files = storedContents[storedContents.Count - 1];
			ContentView.Popup.Show(files);
			storedContents.Remove(files);
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

			/*
			 * We can safely assume the root folder of the style, not the repository,
			 * contains a config file (currently only supports project.json)
             * and therefore can remove any other folder hierarchy
             * 
             * TODO What if it always isn't like that? && support other types of config files
             * 
			 */

			foreach (DownloadedGithubFile file in files)
			{
				FileUtils.SaveToAppFolder(file.Stream, file.Path, file.Name);
			}


			Device.BeginInvokeOnMainThread(delegate
			{
				ContentView.HideLoading();
			});
		}

		public void OnGithubFileDownloadComplete(object sender, EventArgs e)
		{
			string name = (string)sender;
			Toast.Show("Downloading: " + name);
		}

		void OnDownloadStarted(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				ContentView.Popup.Hide();
				ContentView.ShowLoading();
			});
		}

		void OnFileDownloadComplete(object sender, DownloadEventArgs e)
		{
			List<string> result = FileUtils.SaveToAppFolder(e.Stream, e.Name);

			Device.BeginInvokeOnMainThread(async delegate
			{
				ContentView.HideLoading();
				await Navigation.PushAsync(new MainController(result[1], result[0]));
			});
		}

		const string GithubOwner = "CartoDB";
		const string GithubRepo = "mobile-styles";

		async void OnGithubButtonClick(object sender, EventArgs e)
		{
			ContentView.ShowLoading();
			var contents = await HubClient.Instance.GetRepositoryContent(GithubOwner, GithubRepo);
			OnListDownloadComplete(null, new ListDownloadEventArgs { GithubFiles = contents.ToGithubFiles() });
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
            OnListDownloadComplete(null, new ListDownloadEventArgs { Items = files });
#endif
		}

		void OnDatabaseButtonClick(object sender, EventArgs e)
		{
			string path = Parser.LocalStyleLocation;
            string[] files = { };

            try
            {
                files = Directory.GetFiles(path);
            } catch (DirectoryNotFoundException)
            {
                Alert("Local style directory doesn't seem to exist");
                return;
            }

			Device.BeginInvokeOnMainThread(delegate
			{
				ContentView.Popup.Show(files.ToStoredStyles());
			});
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
                    OnFileDownloadComplete(null, new DownloadEventArgs { Name = item.File.Name, Stream = stream });
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
					// TODO save previous folder and download new files from path
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

	}
}
