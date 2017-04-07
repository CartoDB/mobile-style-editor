
using System;
using Xamarin.Forms;

#if __IOS__
using Xamarin.Forms.Platform.iOS;
#elif __ANDROID__
using Xamarin.Forms.Platform.Android;
#endif

namespace mobile_style_editor
{
	public class CSSEditorView : BaseView
	{
		public BaseView Tabs { get; private set;}
		public BaseView LineCount { get; private set; }
		public EditorField Field { get; private set; }

		public CSSEditorView()
		{
			Tabs = new BaseView();
			Tabs.BackgroundColor = Color.Yellow;

			LineCount = new BaseView();
			LineCount.BackgroundColor = Color.Red;

			Field = new EditorField();
		}

		public override void LayoutSubviews()
		{
			double tabHeight = Height / 10;

			double x = 0;
			double y = 0;
			double w = Width;
			double h = tabHeight;

			AddSubview(Tabs, x, y, w, h);

			y += h;
			w = 0;//Width / 15;
			h = Height - tabHeight;

			AddSubview(LineCount, x, y, w, h);

			x += w;
			w = Width - w;

			AddSubview(Field.ToView(), x, y, w, h);

		}
	}
}
