using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class PickerViewItem : BaseView
	{
		public EventHandler<EventArgs> Click;

		public Image Image { get; private set; }

		public PickerViewItem(string resource)
		{
			BackgroundColor = Color.FromRgb(230, 230, 230);

			Image = new Image();
			Image.Source = ImageSource.FromFile(resource);

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
			double x = padding;
			double y = padding;
			double w = Width - 2 * padding;
			double h = Height - 2 * padding;

			AddSubview(Image, x, y, w, h);
		}
	}
}
