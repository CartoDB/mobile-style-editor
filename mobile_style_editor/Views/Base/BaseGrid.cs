
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class BaseGrid : ScrollView
	{
		Grid content;

		int left;
		int top;

		public int ItemsPerRow { get; set; }

		public BaseGrid()
		{
			content = new Grid();

			ItemsPerRow = 5;

			content.RowDefinitions.Add(new RowDefinition {  Height = new GridLength(1, GridUnitType.Star) });
			content.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });

			Content = content;
		}

		public void AddSubview(View view)
		{
			content.Children.Add(view, left, top);

			if (left == ItemsPerRow)
			{
				left = 0;
				top += 1;
			}
			else
			{
				left += 1;
			}
		}

		public void Add(List<View> views)
		{
			foreach (View view in views)
			{
				AddSubview(view);
			}
		}

		public void Clear()
		{
			content.Children.Clear();
		}
	}
}
