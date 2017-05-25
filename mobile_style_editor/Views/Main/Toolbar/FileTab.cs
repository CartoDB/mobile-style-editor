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
            Text = text.ToUpper();

			Normalize();

			FontSize = 10f;
            //FontFamily = "Courier New";
            FontAttributes = FontAttributes.Bold;

			VerticalTextAlignment = TextAlignment.Center;
			HorizontalTextAlignment = TextAlignment.Center;

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

		public void Highlight()
		{
			TextColor = highlightedColor;
		}

		public void Normalize()
		{
			TextColor = normalizedColor;
		}

		public void UpdateLayout(double x, double y, double w, double h)
		{
			var constraint = BoundsConstraint.FromExpression(() => new Rectangle(x, y, w, h), new View[0]);
			RelativeLayout.SetBoundsConstraint(this, constraint);

			if (Parent != null && Parent is RelativeLayout)
			{
				(Parent as RelativeLayout).ForceLayout();
			}
		}

	}
}
