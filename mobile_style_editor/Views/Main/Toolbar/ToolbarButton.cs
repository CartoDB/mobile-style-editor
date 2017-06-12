using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
    public class ToolbarButton : ImageButton
	{
		Label label;

        public ToolbarButton() : base()
        {
            BackgroundColor = Colors.CartoNavyTransparentDark;
        }

		public ToolbarButton(string text)
		{
			label = new Label();
			label.Text = text;
			label.FontFamily = "Helvetica Neue";
			label.FontSize = 12;
            label.FontAttributes = FontAttributes.Bold;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.HorizontalTextAlignment = TextAlignment.Center;
			label.BackgroundColor = Colors.CartoRed;
			label.TextColor = Color.White;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			//double x = 0;
			//double y = 0;
			//double w = Width;
			//double h = Height;

			//AddSubview(label, x, y, w, h);
		}

	}
}
