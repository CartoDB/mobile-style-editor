
using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Styles;
using Carto.Ui;
using Carto.Utils;
using Carto.VectorTiles;
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
	public class MainView : BaseView
	{
		public Toolbar Toolbar { get; private set; }

		public MapContainer MapView { get; private set; }

		public CSSEditorView Editor { get; private set; }

		public ConfirmationPopup Popup { get; private set; }

		public FileTabPopup FileTabs { get; private set; }

		public ZoomControl Zoom { get; private set; }

		public MainView()
		{
			Toolbar = new Toolbar();

			MapView = new MapContainer();

			Editor = new CSSEditorView();

			Popup = new ConfirmationPopup();

			FileTabs = new FileTabPopup();
#if __UWP__
            Zoom = new ZoomControl();
#endif
		}

		double toolbarHeight;

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			toolbarHeight = 60;

			double x = 0;
			double y = 0;
			double w = Width;
			double h = toolbarHeight;
			double min = 50;

			if (h < min)
			{
				h = min;
			}

			AddSubview(Toolbar, x, y, w, h);

			double mapWidth = Width / 3 * 1.9;
			double mapHeight = Height - h;
			y += h;
			w = mapWidth;
			h = mapHeight;

			AddSubview(MapView, x, y, w, h);

			x += w;
			w = Width - w;
			AddSubview(Editor, x, y, w, h);

			if (Data != null)
			{
				double rowCount = Math.Ceiling((double)Data.DecompressedFiles.Count / 3);
				double rowHeight = Toolbar.Height;

				h = rowCount * rowHeight;
				w = Width / 3;
				x = 0;
				y = rowHeight;

				FileTabs.RowCount = rowCount;

				AddSubview(FileTabs, x, y, w, h);

				Editor.Initialize(Data);
				Toolbar.Initialize(Data);
				FileTabs.Initialize(Data);
			}

			AddSubview(Popup, 0, 0, Width, Height);

#if __UWP__
            double zoomPadding = 15;
            w = mapWidth / 5;
            h = w / 3;
            x = mapWidth - (w + zoomPadding);
            y = Height - (h + zoomPadding);

            AddSubview(Zoom, x, y, w, h);
#endif
		}

		ZipData Data;

		public void Initialize(ZipData data)
		{
			Data = data;
			LayoutSubviews();
		}

		public void ToggleTabs()
		{
			bool willExpand = FileTabs.Toggle();

			if (willExpand)
			{
				Toolbar.ExpandButton.UpdateText(FileTabs.CurrentHighlight);
			}

			Toolbar.ExpandButton.UpdateImage();
		}

		public void UpdateMap(byte[] data, Action completed)
		{
			MapView.Update(data, completed);
		}

		double editorOriginalHeight;

		public void Redraw()
		{
			Toolbar.UpdateLayout(Toolbar.X, Toolbar.Y, Toolbar.Width, toolbarHeight);
			Editor.UpdateLayout(Editor.X, Editor.Y + toolbarHeight, Editor.Width, editorOriginalHeight);

			ForceLayout();
		}

		public void RedrawForKeyboard(double keyboardHeight)
		{
			editorOriginalHeight = Editor.Height;

			Toolbar.UpdateLayout(Toolbar.X, Toolbar.Y, Toolbar.Width, 0);
			Editor.UpdateLayout(Editor.X, Editor.Y - toolbarHeight, Editor.Width, Editor.Height - keyboardHeight + toolbarHeight);
		}
	}

	/*
	 * Container class to fix view hierarchy
     *
	 * mapView.ToView() caused the map to always be on top of sibling views,
	 * if we place it in a contain, it won't have any siblings to be on top of
	 */
	public class MapContainer : BaseView
	{
		MapView mapView;

		public float Zoom { get { return mapView.Zoom; } set { mapView.Zoom = value; } }

		public MapContainer()
		{

			mapView = new MapView(
#if __ANDROID__
			Forms.Context
#endif
			);
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
		}

		public void Update(byte[] data, Action completed)
		{
			mapView.Update(data, completed);
		}

		public void SetZoom(float zoom, float duration)
		{
			mapView.SetZoom(zoom, duration);
		}

	}
}