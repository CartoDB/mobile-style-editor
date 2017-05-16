using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class FileListPopupContent : BasePopupContent
	{
		public EventHandler<EventArgs> ItemClick;

		public override void LayoutSubviews()
		{
			double padding = 0;

			int itemsPerRow = 7;

			if (!IsTablet)
			{
				itemsPerRow = 5;
			}

			double x = padding;
			double y = padding;
			double w = Width / itemsPerRow;
			double h = w * 1.2;

			foreach (var view in Children)
			{
				if (!(view is FileListPopupItem))
				{
					continue;
				}

				var item = (FileListPopupItem)view;

				item.UpdateLayout(x, y, w, h);

				if (Width - (x + w) < 1)
				{
					x = padding;
					y += h + padding;
				}
				else
				{
					x += w + padding;
				}
			}

		}

		public void Populate(List<object> items)
		{
			Children.Clear();

			foreach (object item in items)
			{
				FileListPopupItem view;

				if (item is DriveFile)
				{
					view = new FileListPopupItem((DriveFile)item);
				}
				else if (item is StoredStyle)
				{
					view = new FileListPopupItem((StoredStyle)item);
				}
				else
				{
					view = new FileListPopupItem((GithubFile)item);
				}

				view.Click += OnItemClick;

				AddSubview(view);
			}

			LayoutSubviews();
		}

		void OnItemClick(object sender, EventArgs e)
		{
			if (ItemClick != null)
			{
				ItemClick(sender, e);
			}
		}
	}

}
