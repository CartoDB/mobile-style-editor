
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
		public ExpandButton ExpandButton { get; set; }

		public ToolbarButton UploadButton { get; private set; }

		public ToolbarButton SaveButton { get; private set; }

		public Toolbar()
		{
			BackgroundColor = Colors.CartoNavy;

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

			//AddSubview(UploadButton, x, y, w, h);

			x += w + padding;

			//AddSubview(SaveButton, x, y, w, h);
		}

		public void Initialize(ZipData data)
		{
			ExpandButton.Update(data.StyleFileNames[0]);
		}

	}
}
