using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class FileTab : Label
	{
		public int Index { get; private set; }

		public EventHandler<EventArgs> Tapped;

		Color highlightedColor = Color.White;
		Color normalizedColor = Color.Gray;

		public bool IsHighlighted
		{
			get { return TextColor.Equals(highlightedColor); }
		}

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
			TextColor = highlightedColor;
		}

		public void Normalize()
		{
			TextColor = normalizedColor;
		}
	}
}
