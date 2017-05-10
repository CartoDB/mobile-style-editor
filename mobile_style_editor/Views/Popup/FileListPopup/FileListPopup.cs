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
			}
			else
			{
				horizontalPadding = Width / 15;
			}

			verticalPadding = 70;

			double x = horizontalPadding;
			double y = verticalPadding;

			double contentHeight = Height - 2 * verticalPadding;
			double contentWidth = Width - 2 * horizontalPadding; ;

			double w = contentWidth;
			double h = contentHeight;

			AddSubview(Content, x, y, w, h);

			double padding = 10;

			w = verticalPadding;
			h = verticalPadding - 3 * padding;
			x = horizontalPadding;
			y = verticalPadding - h;

			AddSubview(BackButton, x, y, w, h);

			w = 3 * h;
			x = horizontalPadding + contentWidth - w;
			y = verticalPadding - h;

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
		Image image;

		public BackButton()
		{
			BackgroundColor = Colors.CartoNavy;

			image = new Image();

			string folder = "";
#if __UWP__
			folder = "Assets/";
#endif
			image.Source = ImageSource.FromFile(folder + "icon_arrow_back.png");
		}

		public override void LayoutSubviews()
		{
			double padding = Height / 5;

			double w = Height - 2 * padding;
			double h = w;
			double x = Width / 2 - w / 2;
			double y = padding;

			AddSubview(image, x, y, w, h);
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
