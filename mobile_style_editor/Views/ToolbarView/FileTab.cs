using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class FileTab : Label
	{
		public int Index { get; private set; }

		public EventHandler<EventArgs> Tapped;

		public FileTab(string text, int index)
		{
			Index = index;
			Text = text;

			Normalize();

			FontSize = 12f;
			FontFamily = "Courier New";

			VerticalTextAlignment = TextAlignment.Center;
			HorizontalTextAlignment = TextAlignment.Center;

			TapGestureRecognizer recognizer = new TapGestureRecognizer();
			recognizer.Tapped += delegate {
				if (Tapped != null)
				{
					Tapped(this, EventArgs.Empty);
				}
			};

			GestureRecognizers.Add(recognizer);
		}

		public void Highlight()
		{
			TextColor = Color.White;
		}

		public void Normalize()
		{
			TextColor = Color.Gray;
		}
	}
}
