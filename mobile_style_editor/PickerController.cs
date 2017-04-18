using System;
using System.Collections.Generic;
using System.IO;
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
			iOS.GoogleClient.Instance.Authenticate();
#endif
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

#if __ANDROID__
			(Forms.Context as Droid.MainActivity).SetIsLandscape(true);
#endif
			ContentView.Drive.Click += OnDriveButtonClick;

#if __ANDROID__
			DriveClient.Instance.DownloadComplete += OnDownloadComplete;
#elif __IOS__
			iOS.GoogleClient.Instance.DownloadComplete += OnDownloadComplete;
			ContentView.Popup.Content.ItemClick += OnItemClicked;
#endif
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			ContentView.Drive.Click -= OnDriveButtonClick;

#if __ANDROID__
			DriveClient.Instance.DownloadComplete -= OnDownloadComplete;
#elif __IOS__
			iOS.GoogleClient.Instance.DownloadComplete -= OnDownloadComplete;
			ContentView.Popup.Content.ItemClick -= OnItemClicked;
#endif
		}

		void OnDownloadComplete(object sender, DownloadEventArgs e)
		{
			List<string> result = FileUtils.SaveToAppFolder(e.Stream, e.Name);

			Device.BeginInvokeOnMainThread(async delegate
			{
				await Navigation.PushAsync(new MainController(result[1], result[0]));
			});
		}

		void OnDriveButtonClick(object sender, EventArgs e)
		{
#if __ANDROID__
			DriveClient.Instance.Register(Forms.Context);
			DriveClient.Instance.Connect();
#elif __IOS__
			List<DriveFile> files = iOS.GoogleClient.Instance.GetStyleList();
			ContentView.Popup.Show(files);
#endif
		}

#if __IOS__
		void OnItemClicked(object sender, EventArgs e)
		{
			FileListPopupItem item = (FileListPopupItem)sender;
			iOS.GoogleClient.Instance.DownloadStyle(item.File.Id, item.File.Name);
		}
#endif

	}
}
