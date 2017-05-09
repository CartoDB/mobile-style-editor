using System;
using System.Collections.Generic;
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

			AddSubview(styleList, 0, 0, Width / 2, Height / 1.5);
		}

		public void ShowSampleStyles(List<DownloadResult> results)
		{
			double h = 70;
			foreach (DownloadResult result in results)
			{
				styleList.AddSubview(new StyleListItem(result.CleanName), h);
			}
		}
	}

}
