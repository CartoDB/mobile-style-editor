using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class FileListPopupItem : ClickView
	{
		public DriveFile DriveFile { get; set; }
		public GithubFile GithubFile { get; set; }
		public new StoredStyle Style { get; set; }

		BaseView container;

		Image image;
		Label text;

        public FileListPopupItem()
        {
			container = new BaseView();

			image = new Image();

			text = new Label();
			text.FontSize = 12f;
			text.HorizontalTextAlignment = TextAlignment.Center;
		}

		public void Initialize()
		{
            string folder = "";
#if __UWP__
            folder = "Assets/";
#endif
			if (GithubFile != null)
			{
				/* If we're dealing with a Github file, it can be any type, else it'll be a .zip
				 */

				if (!GithubFile.IsDirectory)
				{
					Disable();
					if (GithubFile.IsZip)
					{
						image.Source = ImageSource.FromFile(folder + "icon_zip.png");
					}
					else
					{
						image.Source = ImageSource.FromFile(folder + "icon_text.png");
					}
				}
				else
				{
					image.Source = ImageSource.FromFile(folder + "icon_folder.png");
				}

			}
			else
			{
				image.Source = ImageSource.FromFile(folder + "icon_zip.png");
			}

			if (!IsTablet)
			{
				text.FontSize = 9;
			}

			if (DriveFile != null)
			{
				text.Text = DriveFile.Name;
			}
			else if (Style != null)
			{
				text.Text = Style.Name;
			}
			else
			{
				text.Text = GithubFile.Name;
			}
		}

        public void Reset()
        {
            DriveFile = null;
            GithubFile = null;
            Style = null;
        }

		public override void LayoutSubviews()
		{
			double containerPadding = Width / 20;

			double x = containerPadding;
			double y = containerPadding;
			double w = Width - 2 * containerPadding;
			double h = Height - 2 * containerPadding;

			AddSubview(container, x, y, w, h);

			double imagePadding = container.Width / 5;
			double textPadding = container.Width / 20;

			x = imagePadding;

			y = textPadding; // doesn't need that much y
			w = Width - 2 * imagePadding;
			h = w;

			container.AddSubview(image, x, y, w, h);

			y += h + textPadding;

			x = 0;
			h = Height - (h + textPadding);
			w = container.Width;

			container.AddSubview(text, x, y, w, h);
		}
	}
}
