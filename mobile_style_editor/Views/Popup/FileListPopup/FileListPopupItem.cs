using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class FileListPopupItem : BaseView
	{
		public EventHandler<EventArgs> Click;

		public DriveFile File { get; private set; }
		public new StoredStyle Style { get; private set; }

		BaseView container;

		Image image;
		Label text;

		public FileListPopupItem(DriveFile file)
		{
			File = file;

			Initialize();
		}

		public FileListPopupItem(StoredStyle style)
		{
			Style = style;

            Initialize();
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

			Color color = Color.FromRgb(240, 240, 240);
			container.BackgroundColor = color;

			image = new Image();
			image.Source = ImageSource.FromFile("icon_zip.png");

			text = new Label();
			text.FontSize = 12f;
			text.HorizontalTextAlignment = TextAlignment.Center;

			if (File == null)
			{
				text.Text = Style.Name;
			}
			else
			{
				text.Text = File.Name;
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
