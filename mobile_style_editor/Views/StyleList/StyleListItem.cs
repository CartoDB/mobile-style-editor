
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
	public class StyleListItem : BaseView
	{
		MapView mapView;
		Label label;

		public StyleListItem()
		{
			BackgroundColor = Colors.CartoRedDark;

			label = new Label();
			label.VerticalTextAlignment = TextAlignment.Center;
			label.TextColor = Color.White;
			label.FontSize = 11;
			
			mapView = new MapView();
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

			AddSubview(mapView.ToView(), x, y, w, h);

			y += h + padding;
			h = unitHeight;

			AddSubview(label, x, y, w, h);
		}

		public void Update(string title, byte[] data)
		{
			mapView.Update(data, null);
			label.Text = title.ToUpper();
		}
	}
}
