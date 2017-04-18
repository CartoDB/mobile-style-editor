using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class FileListPopupContent : BasePopupContent
	{
		public EventHandler<EventArgs> ItemClick;

		List<DriveFile> files;

		public void Populate(List<DriveFile> files)
		{
			Children.Clear();

			this.files = files;

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

			foreach (DriveFile file in files)
			{
				FileListPopupItem item = new FileListPopupItem(file);
				item.Click += OnItemClick;

				AddSubview(item, x, y, w, h);

				if (w - y < 1)
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
