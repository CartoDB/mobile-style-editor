
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class MainController : ContentPage
	{
		MainView ContentView;

		public MainController()
		{
			ContentView = new MainView();
			Content = ContentView;
		}

	}
}
