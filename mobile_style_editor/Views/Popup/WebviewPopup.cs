
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class WebviewPopup : BasePopup
	{
		WebView webView;

		public WebviewPopup()
		{
			webView = new WebView();
			webView.Navigating += OnNavigating;
			webView.Navigated += OnNavigationEnd;

			Content = new BasePopupContent();
		}

		public override void LayoutSubviews()
		{
			Content.AddSubview(webView, Content.X, Content.Y, Content.Width, Content.Height);
		}

		public void Open(string url)
		{
			webView.Source = url;
		}
		void OnNavigating(object sender, WebNavigatingEventArgs e)
		{
			Console.WriteLine(e);
		}

		void OnNavigationEnd(object sender, WebNavigatedEventArgs e)
		{
			Console.WriteLine(e.Result);
		}
	}
}
