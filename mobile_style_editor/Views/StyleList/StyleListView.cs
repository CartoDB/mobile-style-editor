
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
    public class StyleListView : ContentView
	{
		BaseScrollView container;

		public StyleContainer MyStyles { get; private set; }

		public StyleContainer Templates { get; private set; }

		public AddStyleItem AddStyle { get; private set; }

		public FileListPopup Popup { get; private set; }
		public WebviewPopup Webview { get; private set; }

		public StyleTabBar Tabs { get; private set; }

		public StyleListView()
		{
            IsNavigationBarVisible = true;

			container = new BaseScrollView();
			container.Orientation = ScrollOrientation.Horizontal;

			BackgroundColor = Colors.CartoRed;

			MyStyles = new StyleContainer();
			MyStyles.Header = StyleContainer.GetHeaderLabel("MY STYLES");

			Templates = new StyleContainer();
			Templates.Header = StyleContainer.GetHeaderLabel("TEMPLATE STYLES");
            Templates.ContainsRefreshButton = true;

			AddStyle = new AddStyleItem();
            MyStyles.Footer = AddStyle;

            Tabs = new StyleTabBar();

			Popup = new FileListPopup();
			Webview = new WebviewPopup();
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			double tabHeight = 0;

			double x = 0;
			double y = 0;
			double w = Width / 2;
			double h = Height;

			if (IsSmallScreen)
			{
				tabHeight = 50;

				w = Width;
				h = Height - tabHeight;
			}

			AddSubview(container, x, y, Width, h);

			container.AddSubview(MyStyles, x, y, w, h);

			x += w;

			container.AddSubview(Templates, x, y, w, h);

			x = 0;
			y = Height - tabHeight;
			h = tabHeight;

			AddSubview(Tabs, x, y, w, h);

			AddSubview(Popup, 0, 0, Width, Height);
			AddSubview(Webview, 0, 0, Width, Height);
		}

		public void HideMapViews()
		{
#if __ANDROID__
			foreach (StyleListItem view in MyStyles.Items)
			{
				view.MapView.Alpha = 0f;
				view.MapView.Visibility = Android.Views.ViewStates.Gone;
			}

			foreach (StyleListItem view in Templates.Items)
			{
				view.MapView.Alpha = 0f;
				view.MapView.Visibility = Android.Views.ViewStates.Gone;
			}
#endif
		}

		public void ShowMapViews()
		{
#if __ANDROID__
			foreach (StyleListItem view in MyStyles.Items)
			{
				view.MapView.Visibility = Android.Views.ViewStates.Visible;
				view.MapView.AnimateAlpha(1.0f);
			}

			foreach (StyleListItem view in Templates.Items)
			{
				view.MapView.Visibility = Android.Views.ViewStates.Visible;

				view.MapView.AnimateAlpha(1.0f);
			}
#endif
		}

		public void OpenWebviewPopup(GithubAuthenticationData data)
		{
			Webview.Show();
			Webview.Open(data);
		}

		public void OpenWebviewPopup(string url)
		{
			Webview.Show();
			Webview.Open(url);
		}

		public void ScrollTo(StyleTab styleTab)
		{
			double x = styleTab.X;
			double y = 0;

			if ((int)x != 0)
			{
				x += styleTab.Width;
			}
			container.ScrollToAsync(x, y, true);
		}
	}
}
