using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class FileListPopupItem : BaseView
	{
        public new bool IsEnabled { get; private set; } = true;

		public EventHandler<EventArgs> Click;

		public DriveFile DriveFile { get; private set; }
		public GithubFile GithubFile { get; private set; }
		public new StoredStyle Style { get; private set; }

		BaseView container;

		Image image;
		Label text;

		public FileListPopupItem(GithubFile file)
		{
			GithubFile = file;

			Initialize();
		}

		public FileListPopupItem(DriveFile file)
		{
			DriveFile = file;

			Initialize();
		}

		public FileListPopupItem(StoredStyle style)
		{
			Style = style;

            Initialize();
		}

        public void Disable()
        {
            IsEnabled = false;
            this.FadeTo(0.5);
        }

		void Initialize()
		{
			TapGestureRecognizer recognizer = new TapGestureRecognizer();
			recognizer.Tapped += delegate
			{
				if (Click != null)
				{
					Click(this, EventArgs.Empty);
				}
			};

			GestureRecognizers.Add(recognizer);

			container = new BaseView();

			image = new Image();

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
				}
				image.Source = ImageSource.FromFile(folder + "icon_zip.png");
			}
			else
			{
				image.Source = ImageSource.FromFile(folder + "icon_zip.png");
			}

			text = new Label();
			text.FontSize = 12f;
			text.HorizontalTextAlignment = TextAlignment.Center;

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
