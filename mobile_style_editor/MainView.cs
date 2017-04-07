
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

		public CSSEditorView Editor { get; private set; }

		public MainView()
		{
#if __IOS__
			MapView = new MapView();
#elif __ANDROID__
			MapView = new MapView(Forms.Context);
#endif
			Editor = new CSSEditorView();
		}

		public override void LayoutSubviews()
		{
			double x = 0;
			double y = 0;
			double w = Width / 3 * 1.9;
			double h = Height;

			AddSubview(MapView.ToView(), new Rectangle(x, y, w, h));

			x += w;
			w = Width - w;

			AddSubview(Editor, new Rectangle(x, y, w, h));
		}


		public void UpdateEditor(ZipData data)
		{
			// TODO All .mss files in tabs
			Editor.Field.Update(data.DecompressedFiles[0]);
		}

	}
}
