
using System;
using System.Collections.Generic;
using Xamarin.Forms;

#if __IOS__
using Xamarin.Forms.Platform.iOS;
#elif __ANDROID__
using Xamarin.Forms.Platform.Android;
#endif

namespace mobile_style_editor
{
	public class Toolbar : BaseView
	{
		public FileTabs Tabs { get; private set; }

		public ExpandButton ExpandButton { get; set; }

		public ToolbarButton UploadButton { get; private set; }

		public ToolbarButton SaveButton { get; private set; }

		public Toolbar()
		{
			BackgroundColor = Colors.CartoNavy;

			//Tabs = new FileTabs();
			ExpandButton = new ExpandButton();

			UploadButton = new ToolbarButton("UPLOAD");

			SaveButton = new ToolbarButton("SAVE");
		}

		public override void LayoutSubviews()
		{
			double x = 0;
			double y = 0;
			double w = Width / 3;
			double h = Height;

			AddSubview(ExpandButton, x, y, w, h);

			double padding = 10;

			w = 100;
			h = w / 3;

			x = Width - (2 * w + 3 * padding);
			y = Height / 2 - h / 2;

			AddSubview(UploadButton, x, y, w, h);

			x += w + padding;

			AddSubview(SaveButton, x, y, w, h);
		}

		public void Initialize(ZipData data)
		{
			ExpandButton.Update(data.StyleFileNames[0]);
		}
	}

	public class ExpandButton : ClickView
	{
		Image image;
		Label label;

		string icon_more_path, icon_less_path;

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
			icon_more_path = folder + "icon_expand_more.png";
			icon_less_path = folder + "icon_expand_less.png";
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

		string current_image_path;

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
			if (current_image_path == null)
			{
				image.Source = ImageSource.FromFile(icon_more_path);
				current_image_path = icon_more_path;
			}
			else
			{
				if (current_image_path.Equals(icon_more_path))
				{
					image.Source = ImageSource.FromFile(icon_less_path);
					current_image_path = icon_less_path;
				}
				else
				{
					image.Source = ImageSource.FromFile(icon_more_path);
					current_image_path = icon_more_path;
				}
			}
		}

	}
}
