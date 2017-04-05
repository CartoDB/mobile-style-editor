using System;

using Xamarin.Forms;

namespace mobile_style_editor
{
	public class EditorApplication : Application
	{
		public EditorApplication()
		{
			MainPage = new MainController();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
