using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class ExpandButton : ClickView
	{
		Image image;
		Label label;

		string icon_path;

		public ExpandButton()
		{
			image = new Image();
			label = new Label();
			label.FontSize = 12;
			label.TextColor = Color.White;
			label.VerticalTextAlignment = TextAlignment.Center;

			string folder = "";

#if __UWP__
			folder = "Assets/";
#endif
			icon_path = folder + "icon_expand_more.png";
            BackgroundColor = Colors.CartoNavyTransparentDark;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			double padding = Width / 20;

			double x = padding;
			double y = padding;
			double w = Height - 2 * padding;
			double h = w;

			AddSubview(image, x, y, w, h);

			x += w;
			y = 0;
			w = Width - (w + padding);
			h = Height;

			AddSubview(label, x, y, w, h);
		}

		static readonly double initialRotation = -1;
		static readonly double collapsedRotation = -90;
		static readonly double expandedRotation = 0;

		static double currentRotation = initialRotation;

		public void Update(string filename)
		{
			UpdateText(filename);
			UpdateImage();
		}

		public void UpdateText(string filename)
		{
			label.Text = "CURRENT STYLE: " + filename.ToUpper();
		}

		public void UpdateImage()
		{
			if ((int)currentRotation == (int)initialRotation)
			{
				image.Source = ImageSource.FromFile(icon_path);
				currentRotation = collapsedRotation;
				image.RotateTo(collapsedRotation);
				return;
			}

			if ((int)currentRotation == (int)collapsedRotation)
			{
				currentRotation = expandedRotation;
				image.RotateTo(expandedRotation);
			}
			else
			{
				currentRotation = collapsedRotation;
				image.RotateTo(currentRotation);
			}

		}

	}
}
