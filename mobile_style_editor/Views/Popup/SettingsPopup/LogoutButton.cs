
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class LogoutButton : ClickView
	{
		Image image;
		Label text;

		public LogoutButton()
		{
			image = new Image();
			image.Source = ImageSource.FromFile("icon_logout.png");

			text = new Label();
			text.Text = "LOG OUT";
			text.VerticalTextAlignment = TextAlignment.Center;
			text.HorizontalTextAlignment = TextAlignment.Center;
			text.TextColor = Colors.CartoNavy;
			text.FontAttributes = FontAttributes.Bold;
			text.FontSize = 13;
		}

		public override void LayoutSubviews()
		{
			double padding = Height / 10;
			double imageSize = Height - 2 * padding;

			double x = 0;
			double y = 0;
			double w = Width - (imageSize + 2 * padding);
			double h = Height;

			AddSubview(text, x, y, w, h);

			x += w + padding;
			y = padding;
			w = imageSize;
			h = imageSize;

			AddSubview(image, x, y, w, h);
		}

	}
}
