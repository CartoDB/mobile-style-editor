
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

			ContentView.Editor.SaveButton.Clicked += OnSave;
			ContentView.Editor.Field.EditingEnded += OnSave;

			ContentView.Popup.Content.Confirm.Clicked += OnConfirmButtonClicked;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			ContentView.Toolbar.Tabs.OnTabTap -= OnTabTapped;
			ContentView.Toolbar.UploadButton.Click -= OnUploadButtonClicked;
			ContentView.Toolbar.SaveButton.Click -= OnSaveButtonClicked;

			ContentView.Editor.SaveButton.Clicked -= OnSave;
			ContentView.Editor.Field.EditingEnded -= OnSave;

			ContentView.Popup.Content.Confirm.Clicked -= OnConfirmButtonClicked;
		}

		void OnUploadButtonClicked(object sender, EventArgs e)
		{
			ContentView.Popup.Show(PopupType.Upload);
			ContentView.Popup.Content.Text = currentWorkingName;
		}

		void OnSaveButtonClicked(object sender, EventArgs e)
		{
			if (currentWorkingName == null)
			{
				// TODO Alert -> You don't seem to have made any changes
				return;
			}

			ContentView.Popup.Show(PopupType.Save);
			ContentView.Popup.Content.Text = currentWorkingName.Replace(Parser.ZipExtension, "");
		}

		void OnConfirmButtonClicked(object sender, EventArgs e)
		{
			string name = ContentView.Popup.Content.Text + Parser.ZipExtension;

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
				LocalStorage.Instance.AddStyle(name, Parser.LocalStyleLocation);
			}
		}

		string currentWorkingName;
		MemoryStream currentWorkingStream;

		void OnSave(object sender, EventArgs e)
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
				string name = "updated_" + data.Filename;

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
