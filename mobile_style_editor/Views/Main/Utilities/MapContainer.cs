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
	/*
     * Container class to fix view hierarchy
     *
     * mapView.ToView() caused the map to always be on top of sibling views,
     * if we place it in a container, it won't have any siblings to be on top of
     */
	public class MapContainer : BaseView
	{
		public bool IsZoomVisible { get; set; }

		public Label zoomLabel;

#if __IOS__
		public bool UserInteractionEnabled
		{
			get { return mapView.UserInteractionEnabled; }
			set { mapView.UserInteractionEnabled = value; }
		}
#endif

#if __ANDROID__
        public float Alpha
        {
            get { return mapView.Alpha; }
            set { mapView.Alpha = value; }
        }

        public Android.Views.ViewStates Visibility
        {
            get { return mapView.Visibility; }
            set { mapView.Visibility = value; }
        }
#endif
		MapView mapView;

		public float Zoom { get { return mapView.Zoom; } set { mapView.Zoom = value; } }

		string ZoomText { get { return "ZOOM: " + Math.Round(mapView.Zoom, 1); } }

		public MapContainer()
		{
			mapView = new MapView(
#if __ANDROID__
            Forms.Context
#endif
			);

			mapView.MapEventListener = new MapListener(this);

			zoomLabel = new Label();
			zoomLabel.VerticalTextAlignment = TextAlignment.Center;
			zoomLabel.HorizontalTextAlignment = TextAlignment.Center;
			zoomLabel.TextColor = Color.White;
			zoomLabel.BackgroundColor = Colors.CartoNavyTransparent;
			zoomLabel.FontAttributes = FontAttributes.Bold;
			zoomLabel.FontSize = 12;
			zoomLabel.Text = ZoomText;
		}

		public override void LayoutSubviews()
		{
#if __ANDROID__
            mapView.RemoveFromParent();
#elif __UWP__
            if (mapView.Parent != null)
            {
                (mapView.Parent as NativeViewWrapperRenderer).Children.Remove(mapView);
            }
#endif
			AddSubview(mapView.ToView(), 0, 0, Width, Height);

			if (IsZoomVisible)
			{
				double w = 80;
				double h = 30;

				AddSubview(zoomLabel, Width - w, 0, w, h);
			}
		}

		public void Update(bool withListener, byte[] data, Action completed, Action<string> failed)
		{
			mapView.Update(withListener, data, completed, failed);
		}

		public void SetZoom(float zoom, float duration)
		{
			mapView.SetZoom(zoom, duration);
		}

#if __ANDROID__
        public void AnimateAlpha(float alpha, long duration = 250)
        {
            mapView.Animate().Alpha(alpha).SetDuration(duration).Start();
        }
#endif

		public void OnMapMoved()
		{
			zoomLabel.Text = ZoomText;
		}
	}

	public class MapListener : MapEventListener
	{
		public MapContainer MapView { get; private set; }

		public MapListener(MapContainer map)
		{
			MapView = map;
		}

		public override void OnMapMoved()
		{
			MapView.OnMapMoved();
		}
	}
}
