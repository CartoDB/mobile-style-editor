
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class MainView : BaseView
	{
		public Entry Editor { get; private set; }

		public MainView()
		{
			Editor = new Entry();
			Editor.BackgroundColor = Color.Black;

			AddSubview(Editor);
		}

		public override void LayoutSubviews()
		{
			double x = 0;
			double y = 0;
			double w = Width;
			double h = Height / 2;

			Editor.Layout(new Rectangle(x, y, w, h));
		}
	}
}
