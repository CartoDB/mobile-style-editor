using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class ToolbarButton : BaseView
	{
		public EventHandler<EventArgs> Click;

		Label label;

		public ToolbarButton(string text)
		{
			label = new Label();
			label.Text = text;
			label.FontFamily = "Helvetica Neue";
			label.FontSize = 13;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.HorizontalTextAlignment = TextAlignment.Center;
			label.BackgroundColor = Colors.CartoRed;
			label.TextColor = Color.White;

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
			double x = 0;
			double y = 0;
			double w = Width;
			double h = Height;

			AddSubview(label, x, y, w, h);
		}

	}
}
