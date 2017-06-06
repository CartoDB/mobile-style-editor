
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class BranchHeader : ClickView
	{
		Label label;

		public string Text
		{
			get { return label.Text; }
			set { label.Text = value; }
		}

		public BranchHeader()
		{
			label = new Label();
			label.BackgroundColor = Colors.CartoNavy;
			label.Text = "BRANCH: master";
			label.FontSize = 12;
			label.TextColor = Color.White;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.HorizontalTextAlignment = TextAlignment.Center;
		}

		public override void LayoutSubviews()
		{
			AddSubview(label, 0, 0, Width, Height);
		}
	}
}
