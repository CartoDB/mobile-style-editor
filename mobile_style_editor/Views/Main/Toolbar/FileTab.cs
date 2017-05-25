using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
    public class FileTab : BaseView
	{
        Label label;

		public int Index { get; private set; }

		public EventHandler<EventArgs> Tapped;

		Color highlightedColor = Color.White;
		Color normalizedColor = Color.Gray;

		public bool IsHighlighted
		{
			get { return label.TextColor.Equals(highlightedColor); }
		}

        public string Text { get { return label.Text; } set { label.Text = value; } }

		public FileTab(string text, int index)
		{
			Index = index;

            label = new Label();

            label.Text = text.ToUpper();

			label.FontSize = 10f;
            label.FontAttributes = FontAttributes.Bold;
            label.BackgroundColor = Colors.CartoNavyLight;

			label.VerticalTextAlignment = TextAlignment.Center;
			label.HorizontalTextAlignment = TextAlignment.Center;

            Normalize();

			TapGestureRecognizer recognizer = new TapGestureRecognizer();
			recognizer.Tapped += delegate
			{
				if (Tapped != null)
				{
					Tapped(this, EventArgs.Empty);
				}
			};

			GestureRecognizers.Add(recognizer);
		}

        public override void LayoutSubviews()
        {
            double padding = 3;

            AddSubview(label, padding, 2 * padding, Width - 2 * padding, Height - 4 * padding);
        }

		public void Highlight()
		{
			label.TextColor = highlightedColor;
		}

		public void Normalize()
		{
			label.TextColor = normalizedColor;
		}

	}
}
