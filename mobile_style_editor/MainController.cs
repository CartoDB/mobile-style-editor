
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

			ContentView.Editor.SaveButton.Clicked += OnSave;
			ContentView.Editor.Field.EditingEnded += OnSave;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			ContentView.Toolbar.Tabs.OnTabTap -= OnTabTapped;
			ContentView.Toolbar.UploadButton.Click -= OnUploadButtonClicked;

			ContentView.Editor.SaveButton.Clicked -= OnSave;
			ContentView.Editor.Field.EditingEnded -= OnSave;
		}

		void OnUploadButtonClicked(object sender, EventArgs e)
		{
			ContentView.UploadPopup.Show();
#if __ANDROID__
			DriveClient.Instance.Upload(currentWorkingName, currentWorkingStream);
#elif __IOS__
			iOS.GoogleClient.Instance.Upload(currentWorkingName, currentWorkingStream);
#endif
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
				string path = data.FilePaths[index];

				FileUtils.OverwriteFileAtPath(path, text);
				string name = "updated_" + data.Filename;

				string zipPath = Parser.ZipData(data.DecompressedPath, name);

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
