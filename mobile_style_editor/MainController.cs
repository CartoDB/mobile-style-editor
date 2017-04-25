
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class MainController : ContentPage
	{
		MainView ContentView;

		ZipData data;

		string folder, filename;

		public MainController(string folder, string filename)
		{
			this.folder = folder;
			this.filename = filename;

			ContentView = new MainView();
			Content = ContentView;

			Title = "CARTO STYLE EDITOR";
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

#if __ANDROID__
			(Forms.Context as Droid.MainActivity).SetIsLandscape(true);
#endif
			ContentView.ShowLoading();
			Task.Run(delegate
			{
				data = Parser.GetZipData(folder, filename);
				Device.BeginInvokeOnMainThread(delegate
				{
					ContentView.Initialize(data);
				});

				byte[] zipBytes = FileUtils.PathToByteData(data.DecompressedPath + Parser.ZipExtension);

				Device.BeginInvokeOnMainThread(delegate
				{
					ContentView.UpdateMap(zipBytes);
					ContentView.HideLoading();
				});
			});

			ContentView.Toolbar.Tabs.OnTabTap += OnTabTapped;
			ContentView.Toolbar.UploadButton.Click += OnUploadButtonClicked;
			ContentView.Toolbar.SaveButton.Click += OnSaveButtonClicked;

			ContentView.Editor.RefreshButton.Clicked += OnRefresh;
			ContentView.Editor.Field.EditingEnded += OnRefresh;

			ContentView.Popup.Content.Confirm.Clicked += OnConfirmButtonClicked;
#if __ANDROID__
			DriveClient.Instance.UploadComplete += OnUploadComplete;
#elif __IOS__
			iOS.GoogleClient.Instance.UploadComplete += OnUploadComplete;
#elif __UWP__
#endif
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			ContentView.Toolbar.Tabs.OnTabTap -= OnTabTapped;
			ContentView.Toolbar.UploadButton.Click -= OnUploadButtonClicked;
			ContentView.Toolbar.SaveButton.Click -= OnSaveButtonClicked;

			ContentView.Editor.RefreshButton.Clicked -= OnRefresh;
			ContentView.Editor.Field.EditingEnded -= OnRefresh;

			ContentView.Popup.Content.Confirm.Clicked -= OnConfirmButtonClicked;

#if __ANDROID__
			DriveClient.Instance.UploadComplete += OnUploadComplete;
#elif __IOS__
			iOS.GoogleClient.Instance.UploadComplete -= OnUploadComplete;
#elif __UWP__
#endif
		}

		void NormalizeView(string text)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				ContentView.HideLoading();
				ContentView.Popup.Hide();
				Toast.Show(text);
			});
		}

		void OnUploadComplete(object sender, EventArgs e)
		{
			NormalizeView("Upload of " + (string)sender + " complete");
		}

		void OnUploadButtonClicked(object sender, EventArgs e)
		{
			ShowPopup(PopupType.Upload);
		}

		void OnSaveButtonClicked(object sender, EventArgs e)
		{
			ShowPopup(PopupType.Save);
		}

		void ShowPopup(PopupType type)
		{
			if (currentWorkingName == null)
			{
				Toast.Show("You don't seem to have made any changes");
				return;
			}

			ContentView.Popup.Show(type);
		}

		void OnConfirmButtonClicked(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				ContentView.ShowLoading();

				Task.Run(delegate
				{
					string name = ContentView.Popup.Content.Text;

					if (string.IsNullOrWhiteSpace(name))
					{
						Toast.Show("Please provide a name for your style");
						return;
					}

					name += Parser.ZipExtension;

					if (ContentView.Popup.Type == PopupType.Upload)
					{
#if __ANDROID__
						DriveClient.Instance.Upload(name, currentWorkingStream);
#elif __IOS__
						iOS.GoogleClient.Instance.Upload(name, currentWorkingStream);
#endif
					}
					else
					{
						if (!Directory.Exists(Parser.LocalStyleLocation))
						{
							Directory.CreateDirectory(Parser.LocalStyleLocation);
						}

						LocalStorage.Instance.AddStyle(name, Parser.LocalStyleLocation);
						string source = Path.Combine(Parser.ApplicationFolder, currentWorkingName);
						string destination = Path.Combine(Parser.LocalStyleLocation, name);
						File.Copy(source, destination);

						NormalizeView(name + " saved to local database");
					}
				});
			});

		}

		string currentWorkingName;
		MemoryStream currentWorkingStream;

		void OnRefresh(object sender, EventArgs e)
		{
			int index = ContentView.Toolbar.Tabs.ActiveIndex;
			string text = ContentView.Editor.Text;

			if (index == -1)
			{
				System.Diagnostics.Debug.WriteLine("Couldn't find a single active tab");
				return;
			}

			ContentView.ShowLoading();

			Task.Run(delegate
			{
				string path = data.StyleFilePaths[index];

				FileUtils.OverwriteFileAtPath(path, text);
				string name = "temporary-" + data.Filename;

				string zipPath = Parser.Compress(data.DecompressedPath, name);

				// Get bytes to update style
				byte[] zipBytes = FileUtils.PathToByteData(zipPath);

				Device.BeginInvokeOnMainThread(delegate
				{
					// Save current working data (name & bytes as stream) to conveniently upload
					// Doing this on the main thread to assure thread safety
					currentWorkingName = name;
					currentWorkingStream = new MemoryStream(zipBytes);

					ContentView.UpdateMap(zipBytes);
					ContentView.HideLoading();
				});
			});
		}

		void OnTabTapped(object sender, EventArgs e)
		{
			FileTab tab = (FileTab)sender;

			ContentView.Editor.Update(tab.Index);
		}

	}
}
