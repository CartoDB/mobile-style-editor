using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class BackButton : ClickView
	{
		Image image;

		public BackButton()
		{
			BackgroundColor = Colors.CartoNavy;

			image = new Image();

			string folder = "";
#if __UWP__
			folder = "Assets/";
#endif
			image.Source = ImageSource.FromFile(folder + "icon_arrow_back.png");
		}

		public override void LayoutSubviews()
		{
			double padding = Height / 5;

			double w = Height - 2 * padding;
			double h = w;
			double x = Width / 2 - w / 2;
			double y = padding;

			AddSubview(image, x, y, w, h);
		}
	}
}
