
using System;
using System.Collections.Generic;
using System.IO;
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
	public class StyleListController : ContentPage
	{
		public StyleView ContentView { get; private set; }

		public StyleListController()
		{
			Title = "CARTO STYLE VIEWER";

			ContentView = new StyleView();
			Content = ContentView;
		}

		bool filesDownloaded;
		protected override async void OnAppearing()
		{
			base.OnAppearing();

			if (!filesDownloaded)
			{
				List<DownloadResult> results = await GetContents();
				ContentView.Templates.ShowSampleStyles(results);
				filesDownloaded = true;
			}

			ContentView.AddStyle.Drive.Click += OnDriveButtonClick;
			ContentView.AddStyle.Github.Click += OnGithubButtonClick;

			ContentView.MyStyles.ItemClick += OnStyleClick;
			ContentView.Templates.ItemClick += OnStyleClick;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			ContentView.AddStyle.Drive.Click -= OnDriveButtonClick;
			ContentView.AddStyle.Github.Click -= OnGithubButtonClick;

			ContentView.MyStyles.ItemClick -= OnStyleClick;
			ContentView.Templates.ItemClick -= OnStyleClick;
		}

		void OnDriveButtonClick(object sender, EventArgs e)
		{
			Console.WriteLine("drive");
		}

		void OnGithubButtonClick(object sender, EventArgs e)
		{
			Console.WriteLine("github");
		}

		void OnStyleClick(object sender, EventArgs e)
		{
			StyleListItem item = (StyleListItem)sender;

			Device.BeginInvokeOnMainThread(async delegate
			{
				await Navigation.PushAsync(new MainController(item.Data.Path, item.Data.Filename));
			});
		}

		public async System.Threading.Tasks.Task<List<DownloadResult>> GetContents()
		{
			List<Octokit.RepositoryContent> zipfiles = await HubClient.Instance.GetZipFiles("CartoDB", "mobile-sample-styles");

			List<DownloadResult> results = new List<DownloadResult>();

			foreach (var content in zipfiles)
			{
				bool existsLocally = FileUtils.HasLocalCopy(content.Name);

				string path = "";
				string filename = "";

				if (existsLocally)
				{
					Console.WriteLine("Using local file: " + content.Name);

					path = FileUtils.GetLocalPath();
					filename = content.Name;
				}
				else
				{
					Console.WriteLine("Downloading: " + content.Name);

					var file = await HubClient.Instance.DownloadFile(content);
					List<string> data = FileUtils.SaveToAppFolder(file.Stream, file.Name);

					path = data[1];
					filename = data[0];
				}

				results.Add(new DownloadResult { Path = path, Filename = filename });
			}

			return results;
		}

	}

	public class DownloadResult
	{
		public string Path { get; set; }

		public string Filename { get; set; }

		public string CleanName { get { return Filename.Replace(Parser.ZipExtension, ""); } }

		public byte[] Data
		{
			get
			{
				return FileUtils.PathToByteData(System.IO.Path.Combine(Path, Filename));
			}
		}
	}
}
