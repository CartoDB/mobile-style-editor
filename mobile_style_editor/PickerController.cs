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
			GoogleClient.Instance.Authenticate();
#endif
			Title = "CARTO STYLE EDITOR";
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			ContentView.Drive.Click += OnDriveButtonClick;
			ContentView.Database.Click += OnDatabaseButtonClick;

			ContentView.Popup.FileContent.ItemClick += OnItemClicked;

#if __ANDROID__
			DriveClient.Instance.DownloadStarted += OnDownloadStarted;
			DriveClient.Instance.DownloadComplete += OnFileDownloadComplete;
#elif __IOS__
			GoogleClient.Instance.DownloadComplete += OnFileDownloadComplete;
			GoogleClient.Instance.ListDownloadComplete += OnListDownloadComplete;
#endif
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			ContentView.Drive.Click -= OnDriveButtonClick;
			ContentView.Database.Click -= OnDatabaseButtonClick;

			ContentView.Popup.FileContent.ItemClick -= OnItemClicked;

#if __ANDROID__
			DriveClient.Instance.DownloadStarted -= OnDownloadStarted;
			DriveClient.Instance.DownloadComplete -= OnFileDownloadComplete;
#elif __IOS__
			GoogleClient.Instance.DownloadComplete -= OnFileDownloadComplete;
			GoogleClient.Instance.ListDownloadComplete -= OnListDownloadComplete;
#endif
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

		void OnDriveButtonClick(object sender, EventArgs e)
		{
#if __ANDROID__
			DriveClient.Instance.Register(Forms.Context);
			DriveClient.Instance.Connect();
#elif __IOS__
			ContentView.ShowLoading();
			GoogleClient.Instance.DownloadStyleList();
#endif
		}

		void OnDatabaseButtonClick(object sender, EventArgs e)
		{
			string path = Parser.LocalStyleLocation;

			string[] files = Directory.GetFiles(path);

			Device.BeginInvokeOnMainThread(delegate
			{
				ContentView.Popup.Show(files.ToStoredStyles());
			});
		}

		void OnListDownloadComplete(object sender, ListDownloadEventArgs e)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				ContentView.Popup.Show(e.Items);
				ContentView.HideLoading();
			});
		}

		async void OnItemClicked(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				ContentView.Popup.Hide();
				ContentView.ShowLoading();
			});

			FileListPopupItem item = (FileListPopupItem)sender;

			if (item.File == null)
			{
				await Navigation.PushAsync(new MainController(item.Style.Path, item.Style.Name));
				Device.BeginInvokeOnMainThread(delegate
				{
					ContentView.HideLoading();
				});
			}
			else
			{
#if __IOS__
				GoogleClient.Instance.DownloadStyle(item.File.Id, item.File.Name);
#endif
			}
		}

	}
}
