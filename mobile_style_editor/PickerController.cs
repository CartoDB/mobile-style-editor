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
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
	
#if __ANDROID__
			(Forms.Context as Droid.MainActivity).SetIsLandscape(true);
#endif
			ContentView.Drive.Click += OnDriveButtonClick;

			DriveClient.Instance.DownloadComplete += OnDownloadComplete;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			ContentView.Drive.Click -= OnDriveButtonClick;

			DriveClient.Instance.DownloadComplete -= OnDownloadComplete;
		}

		void OnDownloadComplete(object sender, DownloadEventArgs e)
		{
			Console.WriteLine(e.Stream);

			List<string> result = FileUtils.SaveToAppFolder(e.Stream, e.Name);

			Device.BeginInvokeOnMainThread(async delegate
			{
				await Navigation.PushAsync(new MainController(result[1], result[0]));
			});
		}

		void OnDriveButtonClick(object sender, EventArgs e)
		{
			DriveClient.Instance.Register(Forms.Context);
			DriveClient.Instance.Connect();
		}
	}
}
