using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class FileListPopup : BasePopup
	{
		public FileListPopupContent FileContent { get { return Content as FileListPopupContent; } }

		public PaginationView Pages { get; private set; }

        public BranchContainer Branches { get; private set; }

        public FileListPopup()
        {
            Content = new FileListPopupContent();

            Pages = new PaginationView();
            Pages.ContentHeight = 40;

            Branches = new BranchContainer();
            Branches.IsVisible = false;
        }

		public override void LayoutSubviews()
		{
            base.LayoutSubviews();

			double w = ContentWidth;
			double h = Pages.ContentHeight;
			double x = ContentX;
			double y = ContentY + ContentHeight;

			AddSubview(Pages, x, y, w, h);

            double padding = 10;

			Branches.HeaderHeight = HeaderHeight;
			Branches.TotalHeight = 400;
            Branches.OriginalY = y + padding;

            x = ContentX;
            y = Branches.OriginalY;
            w = 200;
            h = Branches.HeaderHeight;

            AddSubview(Branches, x, y, w, h);
		}

		public void Show(List<DriveFile> files)
		{
			Show();
			FileContent.Populate(files.ToObjects());
			
            Header.HideButtons();

            Header.Text = "GOOGLE DRIVE";
		}

		public void Show(List<StoredStyle> styles)
		{
			Show();
			FileContent.Populate(styles.ToObjects());
		}

		public List<GithubFile> GithubFiles { get; private set; }

		public void Show(List<GithubFile> files)
		{
            Header.ShowButtons();

			Show();
			GithubFiles = files;
			FileContent.Populate(files.ToObjects());

			if (files.Any(file => file.IsProjectFile))
			{
				Header.Select.Enable();
			}
			else
			{
				Header.Select.Disable();
			}
		}

		public override void Hide(bool animated = true)
		{
			if (Pages != null)
			{
				Pages.Reset();
			}
			base.Hide(animated);
		}
	}
}
