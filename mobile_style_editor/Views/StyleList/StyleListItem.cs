
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

#if __ANDROID__
		ClickView overlay;
#endif
		public MapView MapView { get; private set; }

		Label label;

		public StyleListItem()
		{
			BackgroundColor = Colors.CartoRedDark;

			label = new Label();
			label.VerticalTextAlignment = TextAlignment.Center;
			label.TextColor = Color.White;
			label.FontSize = 11;

			MapView = new MapView(
#if __ANDROID__
				Forms.Context
#endif
			);

#if __ANDROID__
			overlay = new ClickView();
			overlay.Click += delegate {
				Click(this, EventArgs.Empty);
			};
#elif __IOS__
			MapView.UserInteractionEnabled = false;
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

#if __IOS__
            AddSubview(MapView.ToView(), x, y, w, h);
#elif __ANDROID__
			if (MapView.Parent != null)
			{
				MapView.RemoveFromParent();
			}
            AddSubview(MapView.ToView(), x, y, w, h);

			RemoveChild(overlay);
            AddSubview(overlay, x, y, w, h);
#elif __UWP__

            // TODO Crashes application
            if (mapView.Parent == null)
            {
                //AddSubview(mapView.ToView(), x, y, w, h);
            } else
            {
                //mapView.ToView().UpdateLayout(x, y, w, h);
            }
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

			MapView.Update(result.Data, null);
			label.Text = result.CleanName;

			HideLoading();
		}
		
		public void Update(Octokit.RepositoryContent content)
		{
			label.Text = DownloadResult.ToCleanName(content.Name);

			ShowLoading();
		}
	}
}
