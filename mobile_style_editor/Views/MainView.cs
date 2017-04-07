
using System;
using Carto.Ui;
using Xamarin.Forms;

#if __IOS__
using Xamarin.Forms.Platform.iOS;
#elif __ANDROID__
using Xamarin.Forms.Platform.Android;
#endif

namespace mobile_style_editor
{
	public class MainView : BaseView
	{
		public Toolbar Toolbar { get; private set; }

		public MapView MapView { get; private set; }

		public CSSEditorView Editor { get; private set; }

		public MainView()
		{
			Toolbar = new Toolbar();
#if __IOS__
			MapView = new MapView();
#elif __ANDROID__
			MapView = new MapView(Forms.Context);
#endif
			Editor = new CSSEditorView();
		}

		public override void LayoutSubviews()
		{
			int iosPadding = 20;

			double x = 0;
			double y = Device.OnPlatform(iosPadding, 0, 0);
			double w = Width;
			double h = Height / 7;

			AddSubview(Toolbar, x, y, w, h);

			y += h;
			w = Width / 3 * 1.9;
			h = Height - (h + iosPadding);

			AddSubview(MapView.ToView(), new Rectangle(x, y, w, h));

			x += w;
			w = Width - w;

			AddSubview(Editor, new Rectangle(x, y, w, h));

			if (Data != null)
			{
				Editor.Initialize(Data);
				Toolbar.Initialize(Data);
			}
		}

		ZipData Data;

		public void Initialize(ZipData data)
		{
			Data = data;
			Editor.Initialize(Data);
			Toolbar.Initialize(Data);
		}
	}
}
