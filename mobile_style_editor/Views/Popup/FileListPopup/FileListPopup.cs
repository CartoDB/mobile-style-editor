using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class FileListPopup : BasePopup
	{
		public FileListPopupContent FileContent { get { return Content as FileListPopupContent; } }

		public FileListPopup()
		{
			Content = new FileListPopupContent();
		}

		public override void LayoutSubviews()
		{
			double verticalPadding, horizontalPadding;

			if (Width > Height)
			{
				horizontalPadding = Width / 6;
				verticalPadding = Height / 15;
			}
			else
			{
				horizontalPadding = Width / 15;
				verticalPadding = Height / 6;
			}

			double x = horizontalPadding;
			double y = verticalPadding;
			double h = Height - 2 * verticalPadding;
			double w = Width - 2 * horizontalPadding;

			AddSubview(Content, x, y, w, h);
		}

		public void Show(List<DriveFile> files)
		{
			Show();
			(Content as FileListPopupContent).Populate(files.ToObjects());
		}

		public void Show(List<StoredStyle> styles)
		{
			Show();
			(Content as FileListPopupContent).Populate(styles.ToObjects());
		}

	}
}
