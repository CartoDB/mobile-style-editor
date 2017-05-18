
using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace mobile_style_editor
{
	public class PageView : ClickView
	{
		public List<GithubFile> GithubFiles = new List<GithubFile>();

		Label label;

		public string Text { get { return label.Text; } set { label.Text = value; } }

		public PageView()
		{
			label = new Label();
			label.VerticalTextAlignment = TextAlignment.Center;
			label.HorizontalTextAlignment = TextAlignment.Center;
			label.TextColor = Color.White;
			label.FontSize = 12;

			Normalize();
		}

		public override void LayoutSubviews()
		{
			double padding = Height / 10;

			AddSubview(label, padding, padding, Width - 2 * padding, Height - 2 * padding);
		}

		public void Highlight()
		{
			BackgroundColor = Colors.CartoNavy;
			label.FontAttributes = FontAttributes.Bold;
		}

		public void Normalize()
		{
			BackgroundColor = Colors.CartoNavyTransparent;
			label.FontAttributes = FontAttributes.None;
		}
	}
}
