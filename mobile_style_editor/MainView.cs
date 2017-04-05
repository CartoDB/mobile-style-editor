
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
		public MapView MapView { get; private set; }

		public CSSEditor Editor { get; private set; }

		public MainView()
		{
#if __IOS__
			MapView = new MapView();
#elif __ANDROID__
			MapView = new MapView(Forms.Context);
#endif
			Editor = new CSSEditor();
			Editor.BackgroundColor = Color.Black;
		}

		public override void LayoutSubviews()
		{
			double x = 0;
			double y = 0;
			double w = Width / 2;
			double h = Height;

			AddSubview(MapView.ToView(), new Rectangle(x, y, w, h));

			x += w;

			AddSubview(Editor.ToView(), new Rectangle(x, y, w, h));
		}

	}
}
