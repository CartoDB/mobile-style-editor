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
			double padding = 5;

			double x = 0;
			double y = 0;
			double w = styleList.Width;
			double h = 150;

			foreach (DownloadResult result in results)
			{
				var item = new StyleListItem();
				styleList.AddSubview(item, x, y, w, h);
				y += h + padding;

				item.Update(result.CleanName, result.Data);
			}
		}
	}

}
