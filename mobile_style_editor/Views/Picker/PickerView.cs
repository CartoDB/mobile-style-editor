using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class PickerView : BaseView
	{
		public PickerViewItem Drive { get; private set; }

		public FileListPopup Popup { get; private set; }

		public PickerView()
		{
			Drive = new PickerViewItem("icon_drive.png");

			Popup = new FileListPopup();
		}

		public override void LayoutSubviews()
		{
			Popup.Hide();
			double h = Height / 2;

			double max = 300;
			if (h > max)
			{
				h = max;
			}

			double w = h;
			double y = Height / 2 - h / 2;
			double x = Width / 2 - w / 2;

			AddSubview(Drive, x, y, w, h);

			AddSubview(Popup, 0, 0, Width, Height);
		}
	}

	public class FileListPopup : BaseView
	{
		public EventHandler<EventArgs> Click;

		public FileListPopupContent Content { get; private set; }

		public FileListPopup()
		{
			BackgroundColor = Colors.TransparentGray;

			Content = new FileListPopupContent();

			TapGestureRecognizer recognizer = new TapGestureRecognizer();
			recognizer.Tapped += delegate
			{
				if (Click != null)
				{
					Click(this, EventArgs.Empty);
				}
			};

			GestureRecognizers.Add(recognizer);

			Click += OnBackgroundClick;
		}

		void OnBackgroundClick(object sender, EventArgs e)
		{
			Hide();
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
			IsVisible = true;
			Content.Populate(files);
		}

		public void Hide()
		{
			IsVisible = false;
		}
	}

	public class FileListPopupContent : BaseView
	{
		public EventHandler<EventArgs> Click;
		public EventHandler<EventArgs> ItemClick;

		List<DriveFile> files;

		public FileListPopupContent()
		{
			BackgroundColor = Color.White;

			TapGestureRecognizer recognizer = new TapGestureRecognizer();
			recognizer.Tapped += delegate
			{
				if (Click != null)
				{
					Click(this, EventArgs.Empty);
				}
			};

			GestureRecognizers.Add(recognizer);

			Click += OnBackgroundClick;
		}

		void OnBackgroundClick(object sender, EventArgs e)
		{
			// Do nothing. Just here to catch clicks
		}

		public void Populate(List<DriveFile> files)
		{
			Children.Clear();

			this.files = files;

			double topPadding = 10;

			double x = topPadding;
			double y = topPadding;
			double w;

			if (Width > Height)
			{
				w = Height / 6;
			}
			else
			{
				w = Width / 6;
			}

			double h = w * 1.3;

			foreach (DriveFile file in files)
			{
				FileListPopupItem item = new FileListPopupItem(file);
				item.Click += OnItemClick;

				AddSubview(item, x, y, w, h);

				if (w - y < 1)
				{
					x = topPadding;
					y += h + topPadding;
				}
				else
				{
					x += w + topPadding;
				}
			}
		}

		void OnItemClick(object sender, EventArgs e)
		{
			if (ItemClick != null)
			{
				ItemClick(sender, e);
			}
		}
	}

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
