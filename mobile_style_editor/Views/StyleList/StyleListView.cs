using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class StyleListView : BaseView
	{
		BaseScrollView styleList;

		public StyleListView()
		{
			BackgroundColor = Color.Yellow;

			styleList = new BaseScrollView();
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			AddSubview(styleList, 0, 0, Width / 2, Height / 1.2);
		}

		public void ShowSampleStyles(DownloadResult result)
		{
			double h = 100;
			for (int i = 0; i < 50; i++)
			{
				styleList.AddSubview(new Label { Text = i.ToString(), BackgroundColor = Color.Red }, h);
			}
		}
	}

}
