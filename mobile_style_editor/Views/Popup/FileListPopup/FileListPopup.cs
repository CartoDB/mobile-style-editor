using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class FileListPopup : BasePopup
	{
		public FileListPopupContent FileContent { get { return Content as FileListPopupContent; } }

		public FileListHeader Header { get; private set; }

		public FileListPopup()
		{
			Content = new FileListPopupContent();

			Header = new FileListHeader();

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

			w = contentWidth;
			h = verticalPadding - 3 * padding;
			x = horizontalPadding;
			y = verticalPadding - h;

			AddSubview(Header, x, y, w, h);
		}

		public void Show(List<DriveFile> files)
		{
			Show();
			FileContent.Populate(files.ToObjects());
			Header.Select.IsVisible = false;
			Header.BackButton.IsVisible = false;
		}

		public void Show(List<StoredStyle> styles)
		{
			Show();
			FileContent.Populate(styles.ToObjects());
		}

		public List<GithubFile> GithubFiles { get; private set; }

		public void Show(List<GithubFile> files)
		{
			Header.Select.IsVisible = true;
			Header.BackButton.IsVisible = true;

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

	public class FileListHeader : BaseView
	{
		public BackButton BackButton { get; private set; }

		public Label Label { get; private set; }

		public SelectButton Select { get; private set; }

		public void OnBackPress()
		{
			string[] split = Text.Split('/');
			string last = split[split.Length - 1];

			if (last.Equals(""))
			{
				last = split[split.Length - 2];
			}

			Text = Text.Replace(last, "");
		}

		public string Text
		{
			get { return Label.Text; }
			set { Label.Text = value; }
		}

		public FileListHeader()
		{
			Label = new Label();
			Label.VerticalTextAlignment = TextAlignment.Center;
			Label.HorizontalTextAlignment = TextAlignment.Center;
			Label.TextColor = Color.White;
			Label.BackgroundColor = Colors.CartoNavy;
			Label.FontSize = 12;

			BackButton = new BackButton();

			Select = new SelectButton();
		}

		public override void LayoutSubviews()
		{
			double backSize = Height * 2;
			double selectSize = Height * 3;

			double x = 0;
			double y = 0;
			double h = Height;
			double w = backSize;

			AddSubview(BackButton, x, y, w, h);

			x += w;
			w = Width - (backSize + selectSize);

			AddSubview(Label, x, y, w, h);

			x += w;
			w = selectSize;

			AddSubview(Select, x, y, w, h);
		}
	}
}
