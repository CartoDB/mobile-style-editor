
using System;
using System.Collections.Generic;
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
		public StyleListView ContentView { get; private set; }

		public StyleListController()
		{
			ContentView = new StyleListView();
			Content = ContentView;

			GetContents();			
		}

		public async void GetContents()
		{
			List<Octokit.RepositoryContent> zipfiles = await HubClient.Instance.GetZipFiles("CartoDB", "mobile-sample-styles");

			foreach (var content in zipfiles)
			{
				// TODO Check if file exists locally
				bool existsLocally = false;
				if (!existsLocally)
				{
					Console.WriteLine("Downloading: " + content.Name);
					var file = await HubClient.Instance.DownloadFile(content);
					List<string> paths = FileUtils.SaveToAppFolder(file.Stream, file.Name);
					// TODO save local register that file exists

					string path = paths[1];
					string filename = paths[0];

					Device.BeginInvokeOnMainThread(async delegate
					{
						if (content.Name.Equals("nutiteq-bright-blue.zip"))
						{
							Console.WriteLine("Opening MainController: " + path + " & " + filename);
							await Navigation.PushAsync(new MainController(path, filename));
						}
						Console.WriteLine("Name: " + content.Name);
					});
				}
				else
				{
					Console.WriteLine("Using local copy of: " + content.Name);
				}
			}
		}

	}

	public class StyleListView : Frame
	{
		public MapView MapView { get; private set; }

		public StyleListView()
		{
			Padding = new Thickness(0, 0, 0, 0);

			MapView = new MapView();
			Content = MapView.ToView();			
		}
	}
}
