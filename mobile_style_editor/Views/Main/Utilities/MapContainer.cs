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

        public bool IsSourceLabelVisible { get; set; }

        Label zoomLabel;
        
public SourceLabel SourceLabel { get; private set; }

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

            SourceLabel = new SourceLabel();
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
	
            double h = 40;
            double padding = 3;

            if (IsZoomVisible)
            {
                double w = 90;

                AddSubview(zoomLabel, Width - w - padding, padding, w, h);
            }

            if (IsSourceLabelVisible)
            {
                double w = 200;

                AddSubview(SourceLabel, padding, padding, w, h);
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

    public class SourceLabel : BaseView
    {
        Label label;
        Entry field;
        
        public string Text { get { return field.Text;  } set { field.Text = value; } }
        
        public SourceLabel()
        {
            BackgroundColor = Colors.CartoNavyTransparent;

            label = new Label();
            label.Text = "SOURCE";
            label.VerticalTextAlignment = TextAlignment.Center;
            label.HorizontalTextAlignment = TextAlignment.Center;
            label.FontAttributes = FontAttributes.Bold;
            label.FontSize = 12;
            label.TextColor = Color.White;

            field = new Entry();
            field.FontSize = 12;
            field.FontAttributes = FontAttributes.Bold;
            field.BackgroundColor = Colors.CartoNavyTransparent;
            field.TextColor = Color.White;
            
        }

        public override void LayoutSubviews()
        {
            double padding = 5;
            double third = (Width - 3 * padding) / 3;

            double x = padding;
            double y = padding;
            double w = third;
            double h = Height - 2 * padding;

            AddSubview(label, x, y, w, h);

            x += w + padding;
            w = 2 * third;

            AddSubview(field, x, y, w, h);
        }
    }
}
