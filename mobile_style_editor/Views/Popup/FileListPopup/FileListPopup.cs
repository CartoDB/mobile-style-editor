using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class FileListPopup : BasePopup
	{
		public FileListPopupContent FileContent { get { return Content as FileListPopupContent; } }

		public BackButton BackButton { get; private set; }
		public SelectButton Select { get; private set; }

		public FileListPopup()
		{
			Content = new FileListPopupContent();

			BackButton = new BackButton();
			Select = new SelectButton();

			Hide(false);
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

			double padding = 10;

			x = 50;
			y = 50;
			w = 50;
			h = 50;

			AddSubview(BackButton, x, y, w, h);
		
			w = 120;
			h = w / 3;
			x = Content.X + Content.Width - w;
			y = Content.Y + Content.Height + padding;

			AddSubview(Select, x, y, w, h);
		}

		public void Show(List<DriveFile> files)
		{
            Show();
			FileContent.Populate(files.ToObjects());
			Select.IsVisible = false;
			BackButton.IsVisible = false;
		}

		public void Show(List<StoredStyle> styles)
		{
            Show();
			FileContent.Populate(styles.ToObjects());
		}

		public List<GithubFile> GithubFiles { get; private set; }

		public void Show(List<GithubFile> files)
		{
			Select.IsVisible = true;
			BackButton.IsVisible = true;
            
			Show();
			GithubFiles = files;
			FileContent.Populate(files.ToObjects());

			if (files.Any(file => file.IsProjectFile))
			{
				Select.Enable();
			}
			else
			{
				Select.Disable();
			}
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

	public class SelectButton : ClickView
	{
		Label label;

		public SelectButton()
		{
			BackgroundColor = Colors.CartoNavy;

			label = new Label();
			label.FontSize = 13;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.HorizontalTextAlignment = TextAlignment.Center;
			label.TextColor = Color.White;
			label.FontAttributes = FontAttributes.Bold;
			label.Text = "SELECT";
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			AddSubview(label, 0, 0, Width, Height);
		}
			
	}
}
