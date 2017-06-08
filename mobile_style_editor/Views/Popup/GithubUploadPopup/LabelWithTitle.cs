
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
    public class LabelWithTitle : BaseView
    {
		Label title;
		Label label;

		public string Text
		{
			get { return label.Text; }
			set { label.Text = value; }
		}

		public LabelWithTitle(string message)
		{
			title = new Label();
            title.VerticalTextAlignment = TextAlignment.End;
			title.TextColor = Color.Gray;
            title.Text = message;
			title.FontSize = 12f;

			label = new Label();
			label.FontSize = 13f;
            label.TextColor = Colors.CartoNavy;
            label.VerticalTextAlignment = TextAlignment.Center;
            label.FontAttributes = FontAttributes.Bold;
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

			AddSubview(label, x, y, w, h);
		}
    }
}
