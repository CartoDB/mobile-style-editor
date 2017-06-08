
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class EntryWithTitle : BaseView
	{
		Label title;
		Entry input;

		public string Text
		{
			get { return input.Text; }
			set { input.Text = value; }
		}

		public EntryWithTitle(string message)
		{
			title = new Label();
			title.VerticalTextAlignment = TextAlignment.End;
            title.Text = message;
			title.TextColor = Color.Gray;
			title.FontSize = 12f;

            input = new Entry();
			input.FontSize = 13f;
			input.TextColor = Colors.CartoNavy;
		}

		public override void LayoutSubviews()
		{
			double padding = 5;

			double x = padding;
			double y = 0;
			double w = Width - 2 * padding;
			double h = Height / 4;

			AddSubview(title, x, y, w, h);

			y += h;
			h = Height - h;

			AddSubview(input, x, y, w, h);
		}
	}
}
