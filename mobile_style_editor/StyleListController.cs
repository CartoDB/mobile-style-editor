﻿
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

			List<string> paths = new List<string>();
			List<string> filenames = new List<string>();

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

				paths.Add(path);
				filenames.Add(filename);
			}


			ContentView.ShowSampleStyles(paths, filenames);
		}

	}

	public class StyleListView : BaseView
	{
		public StyleListView()
		{
			BackgroundColor = Color.Yellow;
		}

		public void ShowSampleStyles(List<string> paths, List<string> filenames)
		{
			
		}
	}
}