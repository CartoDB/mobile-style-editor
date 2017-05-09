using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class StyleContainer : BaseView
	{
		public BaseView Header { get; set; }

		BaseScrollView styleList;

		public BaseView Footer { get; set; }

		public StyleContainer()
		{
			styleList = new BaseScrollView();
		}

		double padding = 5;

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			double headerHeight = Height / 17;
			double footerHeight = 0;
			double headerPadding = headerHeight / 4;

			if (Footer != null)
			{
				footerHeight = Height / 10;
			}

			double x = padding;
			double y = 0;
			double w = Width - 2 * padding;
			double h = headerHeight;

			if (Header != null)
			{
                AddSubview(Header, x, y, w, h);

				// Separator
				y += h + 3;
				h = 1;
				AddSubview(new BaseView { BackgroundColor = Color.FromRgb(220, 220, 220) }, 2 * padding, y + h, w - 2 * padding, h);
				y += headerPadding;

			}

			y += h + headerPadding;
			h = Height - (headerHeight + headerPadding + footerHeight);

			AddSubview(styleList, x, y, w, h);

			UpdateListLayout();
		}

		public void ShowSampleStyles(List<DownloadResult> results)
		{
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

		void UpdateListLayout()
		{
			double x = 0;
			double y = 0;
			double w = styleList.Width;
			double h = 150;

			foreach (var child in styleList.Children)
			{
				if (child is StyleListItem)
				{
					styleList.AddSubview(child, x, y, w, h);
					y += h + padding;
				}
			}
		}

		public static BaseView GetHeaderLabel(string text)
		{
			BaseView view = new BaseView();
			view.ClearChildrenOnLayout = false;

			Label label = new Label();
			label.Text = text;
			label.VerticalTextAlignment = TextAlignment.End;
			label.TextColor = Color.White;
			label.Margin = new Thickness(0, 0);
			label.FontAttributes = FontAttributes.Bold;
			label.FontSize = 15f;

			view.AddSubview(label, 0, 0, BaseView.MatchParent, BaseView.MatchParent);

			return view;
		}

	}
}
