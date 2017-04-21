using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class FileListPopupContent : BasePopupContent
	{
		public EventHandler<EventArgs> ItemClick;

		public void Populate(List<object> items)
		{
			Children.Clear();

			double padding = 0;

			int itemsPerRow = 6;

			double x = padding;
			double y = padding;
			double w = Width / itemsPerRow;
			double h = w * 1.2;

			foreach (object item in items)
			{
				FileListPopupItem view;

				if (item is DriveFile)
				{
					view = new FileListPopupItem((DriveFile)item);
				}
				else
				{
					view = new FileListPopupItem((StoredStyle)item);
				}

				view.Click += OnItemClick;

				AddSubview(view, x, y, w, h);

				if (Width - (x + w)  < 1)
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

		void OnItemClick(object sender, EventArgs e)
		{
			if (ItemClick != null)
			{
				ItemClick(sender, e);
			}
		}
	}

}
