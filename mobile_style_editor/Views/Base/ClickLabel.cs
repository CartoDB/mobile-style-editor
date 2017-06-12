
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
    public class ClickLabel : ClickView
	{
		public Label Text { get; private set; }

		public ClickLabel()
        {
            Text = new Label();

            Text.VerticalTextAlignment = TextAlignment.Center;
            Text.HorizontalTextAlignment = TextAlignment.Center;
        }

        public override void LayoutSubviews()
        {
            AddSubview(Text, 0, 0, Width, Height);
        }
    }
}
