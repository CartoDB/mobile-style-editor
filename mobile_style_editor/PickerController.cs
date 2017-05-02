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
			DriveClientiOS.Instance.Authenticate();
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

			ContentView.Drive.Click -= OnDriveButtonClick;
			ContentView.Database.Click -= OnDatabaseButtonClick;

			ContentView.Popup.FileContent.ItemClick -= OnItemClicked;

#if __ANDROID__
			DriveClientDroid.Instance.DownloadStarted -= OnDownloadStarted;
			DriveClientDroid.Instance.DownloadComplete -= OnFileDownloadComplete;
#elif __IOS__
			DriveClientiOS.Instance.DownloadComplete -= OnFileDownloadComplete;
			DriveClientiOS.Instance.ListDownloadComplete -= OnListDownloadComplete;
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
				ContentView.Popup.Show(e.Items);
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
				ContentView.Popup.Hide();
				ContentView.ShowLoading();
			});

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
#if __ANDROID__
#elif __IOS__
                DriveClientiOS.Instance.DownloadStyle(item.File.Id, item.File.Name);
#elif __UWP__
                Stream stream = await DriveClientUWP.Instance.DownloadStyle(item.File.Id, item.File.Name);

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
