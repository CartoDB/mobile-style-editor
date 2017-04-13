using System;
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
		}

		void OnDriveButtonClick(object sender, EventArgs e)
		{
			DriveClient.Instance.Register(Forms.Context);
			DriveClient.Instance.Connect();
		}
	}
}
