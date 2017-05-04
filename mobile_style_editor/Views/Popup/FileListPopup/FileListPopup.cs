using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class FileListPopup : BasePopup
	{
		public FileListPopupContent FileContent { get { return Content as FileListPopupContent; } }

		public BackButton BackButton { get; private set; }

		public FileListPopup()
		{
			Content = new FileListPopupContent();

			BackButton = new BackButton();
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
			FileContent.Populate(files.ToObjects());
		}

		public void Show(List<StoredStyle> styles)
		{
            Show();
			FileContent.Populate(styles.ToObjects());
		}

		public List<GithubFile> GithubFiles { get; private set; }

		public void Show(List<GithubFile> files)
		{
            Show();
			GithubFiles = files;
			FileContent.Populate(files.ToObjects());

			AddSubview(BackButton, 50, 50, 50, 50);
		}
	}

	public class BackButton : ClickView
	{
		public BackButton()
		{
			BackgroundColor = Colors.CartoNavy;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
		}
	}
}
