
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

            double padding = 5;

            SaveButton = new ToolbarButton();
            SaveButton.Source = "icon_save.png";
            SaveButton.ImagePadding = padding;

            EmailButton = new ToolbarButton();
            EmailButton.Source = "icon_email.png";
            EmailButton.ImagePadding = padding;

            UploadButton = new ToolbarButton();
            UploadButton.Source = "icon_upload.png";
            UploadButton.ImagePadding = padding;
		}

		public override void LayoutSubviews()
		{
			double padding = 5;

			double x = padding;
            double y = padding;
            double h = Height - 2 * padding;
            double w = h;

            AddSubview(SaveButton, x, y, w, h);

            x += w + padding;

            AddSubview(EmailButton, x, y, w, h);

            if (!isTemplateFolder)
            {
                x += w + padding;

                AddSubview(UploadButton, x, y, w, h);
            }

            if (Tabs.IsVisible)
            {
                x = Width - (Tabs.CalculatedWidth + padding);
                y = 0;
                w = Width - x;
                h = Height;

                AddSubview(Tabs, x, y, Width, h);    
            } 
            else
            {
                x = Width - (250 + padding);
                y = padding;
                w = Width - (x + padding);
                h = Height - 2 * padding;

                AddSubview(ExpandButton, x, y, w, h);    
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

            LayoutSubviews();
        }

	}
}
