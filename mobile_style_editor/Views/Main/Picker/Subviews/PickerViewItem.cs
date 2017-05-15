using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class PickerViewItem : BaseView
	{
		public EventHandler<EventArgs> Click;

		public Image Image { get; private set; }
		Label label;

		public double TextSize
		{ 
			get { return label.FontSize; }
			set { label.FontSize = value; }
		}

		public PickerViewItem(string resource, string text)
		{
			BackgroundColor = Color.FromRgb(240, 240, 240);

			Image = new Image();
			Image.Source = ImageSource.FromFile(resource);

			label = new Label();
			label.Text = text;
			label.TextColor = Colors.CartoNavy;
			label.FontSize = 12f;
			label.HorizontalTextAlignment = TextAlignment.Center;

			TapGestureRecognizer recognizer = new TapGestureRecognizer();
			recognizer.Tapped += delegate
			{
				if (Click != null)
				{
					Click(this, EventArgs.Empty);
				}
			};

			GestureRecognizers.Add(recognizer);
		}

		public override void LayoutSubviews()
		{
			double padding = Height / 10;
			double size = (Width / 3 * 2);

			double x = Width / 2 - size / 2;
			double y = padding;
			double w = size;
			double h = size;

			AddSubview(Image, x, y, w, h);

			y += h + padding / 2;
			x = padding;
			w = Width - 2 * padding;
			h = Height - (h + 2 * padding);

			AddSubview(label, x, y, w, h);
		}
	}
}
