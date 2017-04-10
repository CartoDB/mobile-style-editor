
using System;
using Carto.Core;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class MainController : ContentPage
	{
		MainView ContentView;

		ZipData data;

		public MainController()
		{
			ContentView = new MainView();
			Content = ContentView;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			// TODO decompress on background thread
			data = Parser.GetZipData();
			ContentView.Initialize(data);

			byte[] zipBytes = FileUtils.PathToByteData(data.FolderPath + Parser.ZipExtension);
			ContentView.UpdateMap(zipBytes);
			
			ContentView.Toolbar.Tabs.OnTabTap += OnTabTapped;
			ContentView.Editor.SaveButton.Clicked += OnSave;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			ContentView.Toolbar.Tabs.OnTabTap -= OnTabTapped;
			ContentView.Editor.SaveButton.Clicked -= OnSave;
		}

		void OnSave(object sender, EventArgs e)
		{
			int index = ContentView.Toolbar.Tabs.ActiveIndex;
			string text = ContentView.Editor.Text;

			if (index == -1)
			{
				System.Diagnostics.Debug.WriteLine("Couldn't find a single active tab");
				return;
			}

			string path = data.FilePaths[index];

			FileUtils.OverwriteFileAtPath(path, text);

			string zipPath = Parser.ZipData();

			byte[] zipBytes = FileUtils.PathToByteData(zipPath);

			ContentView.UpdateMap(zipBytes);
		}

		void OnTabTapped(object sender, EventArgs e)
		{
			FileTab tab = (FileTab)sender;

			ContentView.Editor.Update(tab.Index);
		}

	}
}
