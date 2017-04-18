using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class FileListPopupItem : BaseView
	{
		public EventHandler<EventArgs> Click;

		public DriveFile File { get; private set; }
		Image image;
		Label text;

		public FileListPopupItem(DriveFile file)
		{
			File = file;

			TapGestureRecognizer recognizer = new TapGestureRecognizer();
			recognizer.Tapped += delegate
			{
				if (Click != null)
				{
					Click(this, EventArgs.Empty);
				}
			};

			GestureRecognizers.Add(recognizer);

			Color color = Color.FromRgb(240, 240, 240);
			BackgroundColor = color;

			image = new Image();
			image.Source = ImageSource.FromFile("icon_zip.png");

			text = new Label();
			text.Text = file.Name;
			text.FontSize = 12f;
			text.HorizontalTextAlignment = TextAlignment.Center;
			text.VerticalTextAlignment = TextAlignment.Center;
		}

		public override void LayoutSubviews()
		{
			double imagePadding = Width / 5;
			double textPadding = Width / 20;

			double x = imagePadding;
			double y = textPadding; // doesn't need that much y
			double w = Width - 2 * imagePadding;
			double h = w;

			AddSubview(image, x, y, w, h);

			y += h;

			x = textPadding;
			h = Height - (h + textPadding);
			w = Width - 2 * textPadding;

			AddSubview(text, x, y, w, h);
		}
	}
}
