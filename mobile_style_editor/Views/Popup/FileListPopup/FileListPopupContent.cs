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

			double topPadding = 10;

			double x = topPadding;
			double y = topPadding;
			double w;

			if (Width > Height)
			{
				w = Height / 6;
			}
			else
			{
				w = Width / 6;
			}

			double h = w * 1.3;

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

				if (Width - (x + w + topPadding) < 1)
				{
					x = topPadding;
					y += h + topPadding;
				}
				else
				{
					x += w + topPadding;
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
