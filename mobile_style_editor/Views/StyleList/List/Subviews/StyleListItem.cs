
using System;
using Carto.Ui;
using Xamarin.Forms;

#if __IOS__
using Xamarin.Forms.Platform.iOS;
#elif __ANDROID__
using Xamarin.Forms.Platform.Android;
#elif __UWP__
using Xamarin.Forms.Platform.UWP;
#endif

namespace mobile_style_editor
{
	public class StyleListItem : ClickView
	{
		public DownloadResult Data { get; private set; }

#if __IOS__
#else
		ClickView overlay;
#endif
        public MapContainer MapView { get; private set; }

		Label label;

        public StyleListItem()
        {
            BackgroundColor = Colors.CartoRedDark;

            label = new Label();
            label.VerticalTextAlignment = TextAlignment.Center;
            label.TextColor = Color.White;
            label.FontSize = 11;

            MapView = new MapContainer();

#if __IOS__
            MapView.UserInteractionEnabled = false;
#else
            overlay = new ClickView();
            overlay.Click += delegate
            {
                Click(this, EventArgs.Empty);
            };
#endif
        }
        
        public override void LayoutSubviews()
		{
			double padding = 5;

			double divider = 5;
			double unitHeight = (Height - 3 * padding) / divider;

			double x = padding;
			double y = padding;
			double w = Width - 2 * padding;
			double h = unitHeight * (divider - 1);

#if __UWP__
#else
            AddSubview(MapView, x, y, w, h);
#endif

#if __IOS__
#else
            AddSubview(overlay, x, y, w, h);
#endif

            y += h + padding;
			h = unitHeight;

			AddSubview(label, x, y, w, h);
			
			base.LayoutSubviews();
		}

        public void Update(DownloadResult result)
        {
            Data = result;

            label.Text = result.CleanName;
            MapView.Update(false, result.Data, null,
                           (obj) =>
                           {
                               Device.BeginInvokeOnMainThread(delegate
                               {
                                   label.Text = result.CleanName + " (" + obj + ")";
                               });
                           });

            HideLoading();
        }
		
		public void Update(Octokit.RepositoryContent content)
		{
			label.Text = DownloadResult.ToCleanName(content.Name);

			ShowLoading();
		}
	}
}
