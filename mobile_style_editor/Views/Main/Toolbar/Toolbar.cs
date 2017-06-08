
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

		public FileTabs Tabs { get; set; }

		public ToolbarButton UploadButton { get; private set; }

		public ToolbarButton SaveButton { get; private set; }

        public ToolbarButton EmailButton { get; private set; }

        bool isTemplateFolder;

		public Toolbar(bool isTemplateFolder)
		{
            this.isTemplateFolder = isTemplateFolder;

			BackgroundColor = Colors.CartoNavy;

			ExpandButton = new ExpandButton();

            Tabs = new FileTabs();
            Tabs.IsVisible = false;

			SaveButton = new ToolbarButton("SAVE");

            EmailButton = new ToolbarButton("EMAIL");

            UploadButton = new ToolbarButton("UPLOAD");
		}

		public override void LayoutSubviews()
		{
			double x = 0;
			double y = 0;
			double w = Width / 3;
			double h = Height;

			AddSubview(ExpandButton, x, y, w, h);
            AddSubview(Tabs, x, y, Width, h);

			double padding = 10;

            // Button count
            int count = 3;

            if (isTemplateFolder)
            {
                count -= 1;    
            }

			w = 100;
			h = w / 3;
			x = Width - (count * w + count * padding);
			y = Height / 2 - h / 2;

			AddSubview(SaveButton, x, y, w, h);

			x += w + padding;

            AddSubview(EmailButton, x, y, w, h);

            if (!isTemplateFolder)
            {
                x += w + padding;

                AddSubview(UploadButton, x, y, w, h);
            }
		}

        public const int MaxCount = 4;
		/*
         * The default visible view is Expand button, 
         * if there are MaxCount or fewer tabs, show all of them via FileTabs
         * 
         */
		public void Initialize(ZipData data)
        {
            if (data.StyleFileNames.Count <= MaxCount)
            {
                Tabs.IsVisible = true;
                Tabs.Update(data);
                ExpandButton.IsVisible = false;
            }
            else
            {
                ExpandButton.Update(data.StyleFileNames[0]);
            }
        }

	}
}
