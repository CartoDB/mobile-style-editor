
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
			label.FontSize = 13;
			label.TextColor = Colors.CartoNavy;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.HorizontalTextAlignment = TextAlignment.Center;
			label.FontAttributes = FontAttributes.Bold;
		}

		public override void LayoutSubviews()
		{
			AddSubview(label, 0, 0, Width, Height);
		}
	}
}
