
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
			double padding = 20;

			double x = padding;
			double y = padding;
			double w = Width - (2 * padding);
			double h = Height - (2 * padding);

			AddSubview(Content, x, y, w, h);

			x = 0;
			y = 0;

			Content.AddSubview(webView, x, y, w, h);
		}

		public void Open(string url)
		{
			webView.Source = url;
		}

		const string CodeParameter = "?code=";

		void OnNavigationEnd(object sender, WebNavigatedEventArgs e)
		{
			if (e.Result == WebNavigationResult.Success)
			{
				string url = e.Url;

				if (url.Contains(CodeParameter))
				{
					string code = url.Split(new string[] { CodeParameter }, StringSplitOptions.None)[1];
					Console.WriteLine(code);
				}
				else
				{
					// TODO ErrorHandling
					Console.WriteLine(e);
				}
			}
		}
	}
}
