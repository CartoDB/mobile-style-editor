
using System;
using System.Threading.Tasks;
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

#if __ANDROID__
			(Forms.Context as Droid.MainActivity).SetIsLandscape(true);
#endif
			Task.Run(delegate
			{
				data = Parser.GetZipData();
				Device.BeginInvokeOnMainThread(delegate
				{
					ContentView.Initialize(data);
				});

				byte[] zipBytes = FileUtils.PathToByteData(data.FolderPath + Parser.ZipExtension);

				Device.BeginInvokeOnMainThread(delegate
				{
					ContentView.UpdateMap(zipBytes);
				});
			});

			ContentView.Toolbar.Tabs.OnTabTap += OnTabTapped;
			ContentView.Editor.SaveButton.Clicked += OnSave;
			ContentView.Editor.Field.EditingEnded += OnSave;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			ContentView.Toolbar.Tabs.OnTabTap -= OnTabTapped;
			ContentView.Editor.SaveButton.Clicked -= OnSave;
			ContentView.Editor.Field.EditingEnded -= OnSave;
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

			Task.Run(delegate
			{
				string path = data.FilePaths[index];

				FileUtils.OverwriteFileAtPath(path, text);

				string zipPath = Parser.ZipData();

				byte[] zipBytes = FileUtils.PathToByteData(zipPath);

				Device.BeginInvokeOnMainThread(delegate
				{
					ContentView.UpdateMap(zipBytes);
				});
			});
		}

		void OnTabTapped(object sender, EventArgs e)
		{
			FileTab tab = (FileTab)sender;

			ContentView.Editor.Update(tab.Index);
		}

	}
}
