
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class StyleTab : ClickView
	{
		Label label;

		public string Text 
		{ 
			get { return label.Text; } 
			set { label.Text = value; } 
		}

		public StyleTab()
		{
			label = new Label();
			label.TextColor = Colors.CartoNavy;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.HorizontalTextAlignment = TextAlignment.Center;
		}

		public override void LayoutSubviews()
		{
			AddSubview(label, 0, 0, Width, Height);
		}
	}
}
