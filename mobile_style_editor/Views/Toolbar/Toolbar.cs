
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

		public UploadButton UploadButton { get; private set; }

		public Toolbar()
		{
			Tabs = new FileTabs();

			UploadButton = new UploadButton();
		}

		public override void LayoutSubviews()
		{
			double x = 0;
			double y = 0;
			double w = Width;
			double h = Height;

			AddSubview(Tabs, x, y, w, h);

			double padding = 10;

			w = 100;
			h = w / 3;
			x = Width - (w + padding);
			y = padding;

			AddSubview(UploadButton, x, y, w, h);
		}

		public void Initialize(ZipData data)
		{
			Tabs.Update(data);
			Tabs.Highlight(0);
		}

		//public async void Expand()
		//{
		//	Rectangle rect = new Rectangle(X, Y, Width, 1000);
		//	Console.WriteLine(Width + " - " + Height);
		//	//await this.LayoutTo(rect, 300, Easing.CubicIn);
		//	HeightRequest = 1000;
		//	//LayoutTo(rect, 300, Easing.CubicIn);
		//}

		//public void Collapse()
		//{
			
		//}

	}
}
