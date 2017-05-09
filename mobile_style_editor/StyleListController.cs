
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
			var contents = await HubClient.Instance.GetRepositoryContent("CartoDB", "mobile-sample-styles");

			foreach (var content in contents)
			{
				if (content.Name.Contains(".zip"))
				{
					Console.WriteLine("Downloading: " + content.Name);
					// TODO Check if file exists locally
					bool existsLocally = false;
					if (!existsLocally)
					{

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
