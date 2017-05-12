
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class StyleView : BaseScrollView
	{
		public StyleContainer MyStyles { get; private set; }

		public StyleContainer Templates { get; private set; }

		public AddStyleItem AddStyle { get; private set; }

		public FileListPopup Popup { get; private set; }

		public StyleView()
		{
			Orientation = ScrollOrientation.Horizontal;

			BackgroundColor = Colors.CartoRed;

			MyStyles = new StyleContainer();
			MyStyles.Header = StyleContainer.GetHeaderLabel("MY STYLES");

			Templates = new StyleContainer();
			Templates.Header = StyleContainer.GetHeaderLabel("TEMPLATE STYLES");

			AddStyle = new AddStyleItem();
			MyStyles.Footer = AddStyle;

			Popup = new FileListPopup();
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			double x = 0;
			double y = 0;
			double w = Width / 2;
			double h = Height;

			if (!IsTablet || !IsLandscape)
			{
				w = Width;
			}

			if (!IsTablet && IsLandscape)
			{
				w = Width / 2;
			}

			AddSubview(MyStyles, x, y, w, h);

			x += w;

			AddSubview(Templates, x, y, w, h);

			AddSubview(Popup, 0, 0, Width, Height);
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
				view.MapView.Animate().Alpha(1.0f).SetDuration(250).Start();
			}

			foreach (StyleListItem view in Templates.Items)
			{
				view.MapView.Visibility = Android.Views.ViewStates.Visible;

				view.MapView.Animate().Alpha(1.0f).SetDuration(250).Start();
			}
#endif
		}
	}
}
