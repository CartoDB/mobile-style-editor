
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class WebviewPopup : BasePopup
	{
		public EventHandler<AuthenticationEventArgs> Authenticated;

		CustomWebView webView;

		public WebviewPopup()
		{
			webView = new CustomWebView();
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

		string id, secret;

        public void Open(GithubAuthenticationData data)
        {
            id = data.Id;
            secret = data.Secret;

           webView.Source = data.Url;
        }

		public void Open(string url)
		{
			webView.Source = url;
		}

		const string CodeParameter = "?code=";

		void OnNavigationEnd(object sender, WebNavigatedEventArgs e)
		{
			string code = null;
			string error = null;

			if (e.Result == WebNavigationResult.Success)
			{
				string url = e.Url;

				if (url.Contains(CodeParameter))
				{
					code = url.Split(new string[] { CodeParameter }, StringSplitOptions.None)[1];
				}
				else
				{
					error = "Authentication was successful, but unable to parse Authentication Code";
				}
			}
			else
			{
				error = "Authentication failed. Please try again later";
			}

			if (Authenticated != null)
			{
				Authenticated(this, new AuthenticationEventArgs { Id = id, Secret = secret, Code = code, Error = error });
			}
		}

        public void DeleteCookies(string domain)
        {
            webView.DeleteCookies(domain);
        }
	}

	public class AuthenticationEventArgs : EventArgs
	{
		public string Id { get; set; }

		public string Secret { get; set; }

		public string Code { get; set; }

		public string Error { get; set; }

		public bool IsOk { get { return !string.IsNullOrEmpty(Code); } }
	}

}
