
using System;
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
	public class StyleListItem : ClickView
	{
		public DownloadResult Data { get; private set; }

		MapView mapView;
		Label label;

		public MapView MapView { get { return mapView; } }

		public StyleListItem()
		{
			BackgroundColor = Colors.CartoRedDark;

			label = new Label();
			label.VerticalTextAlignment = TextAlignment.Center;
			label.TextColor = Color.White;
			label.FontSize = 11;

			mapView = new MapView(
#if __ANDROID__
				Forms.Context
#endif
			);

#if __ANDROID__
			mapView.Enabled = false;
			mapView.FocusableInTouchMode = false;
#elif __IOS__
			mapView.UserInteractionEnabled = false;
#endif
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			double padding = 5;

			double divider = 5;
			double unitHeight = (Height - 3 * padding) / divider;

			double x = padding;
			double y = padding;
			double w = Width - 2 * padding;
			double h = unitHeight * (divider - 1);

#if __IOS__
#elif __ANDROID__
			if (mapView.Parent != null)
			{
				mapView.RemoveFromParent();
			}
#elif __UWP__

#endif
			AddSubview(mapView.ToView(), x, y, w, h);

			y += h + padding;
			h = unitHeight;

			AddSubview(label, x, y, w, h);
		}

		public void Update(DownloadResult result)
		{
			Data = result;

			mapView.Update(result.Data, null);
			label.Text = result.CleanName.ToUpper();
		}
	}
}
