
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class BaseGrid : Grid
	{
		int left;
		int top;

		public int ItemsPerRow { get; set; }

		public void AddSubview(View view)
		{
			left += 1;
			Children.Add(view, left, top);

			if (left == ItemsPerRow)
			{
				left = 0;
				top += 1;
			}
		}

		public void Add(List<View> views)
		{
			foreach (View view in views)
			{
				AddSubview(view);
			}
		}
	}
}
