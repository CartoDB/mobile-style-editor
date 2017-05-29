
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class NavigationBackButton : ClickView
	{
		Image image;
		Label label;

		public NavigationBackButton()
		{
			label = new Label();
			label.TextColor = Color.White;
			label.Text = "BACK";
			label.FontAttributes = FontAttributes.Bold;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.FontSize = 14;

			image = new Image();

			string folder = "";
#if __UWP__
            folder = "Assets/";
#endif
			image.Source = ImageSource.FromFile(folder + "icon_arrow_back.png");
		}

		public override void LayoutSubviews()
		{
			double imagePadding = Height / 5;
			double labelPadding = 0;

			double x = imagePadding;
			double y = imagePadding;
			double h = Height - 2 * imagePadding;
			double w = h;

			AddSubview(image, x, y, w, h);

			y = labelPadding;
			x += w + imagePadding;
			w = Width - (w + 2 * imagePadding);
			h = Height;

			AddSubview(label, x, y, w, h);
		}
	}
}
