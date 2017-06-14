
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{

	public class SelectButton : ClickView
	{
		Label label;

		public SelectButton()
		{
			BackgroundColor = Colors.CartoNavy;

			label = new Label();
			label.FontSize = 13;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.HorizontalTextAlignment = TextAlignment.Center;
			label.TextColor = Color.White;
			label.FontAttributes = FontAttributes.Bold;
			label.Text = "SELECT";
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			AddSubview(label, 0, 0, Width, Height);
		}

	}
}
