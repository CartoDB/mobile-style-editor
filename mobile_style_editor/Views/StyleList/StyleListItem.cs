
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class StyleListItem : BaseView
	{
		Image image;
		Label label;

		public StyleListItem(string text)
		{
			BackgroundColor = Color.White;

			label = new Label();
			label.VerticalTextAlignment = TextAlignment.Center;
			label.TextColor = Colors.CartoNavy;
			label.Text = text;

			image = new Image();
			image.BackgroundColor = Colors.CartoRed;
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

			AddSubview(image, x, y, w, h);

			y += h + padding;
			h = quarterHeight;

			AddSubview(label, x, y, w, h);
		}

	}
}
