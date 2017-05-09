
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
			BackgroundColor = Color.White;

			label = new Label();
			label.VerticalTextAlignment = TextAlignment.Center;
			label.TextColor = Colors.CartoNavy;

			mapView = new MapView();
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			double padding = 5;
			double quarterHeight = (Height - 3 * padding) / 4;

			double x = padding;
			double y = padding;
			double w = Width - 2 * padding;
			double h = quarterHeight * 3;

			AddSubview(mapView.ToView(), x, y, w, h);

			y += h + padding;
			h = quarterHeight;

			AddSubview(label, x, y, w, h);
		}

		public void Update(string title, byte[] data)
		{
			mapView.Update(data, null);
			label.Text = title;
		}
	}
}
