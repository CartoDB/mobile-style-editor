using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class ClickView : BaseView
	{
		public EventHandler<EventArgs> Click;

		public ClickView()
		{
			IsEnabled = true;

			TapGestureRecognizer recognizer = new TapGestureRecognizer();
			recognizer.Tapped += delegate
			{
				if (!IsEnabled)
				{
					return;
				}

				if (Click != null)
				{
					Click(this, EventArgs.Empty);
				}
			};

			GestureRecognizers.Add(recognizer);
		}

		public void Enable(bool animated = false)
		{
			IsEnabled = true;
            Fade(1.0, animated);
		}

		public void Disable(bool animated = false)
		{
			IsEnabled = false;
			Fade(0.5, animated);
		}

		void Fade(double opacity, bool animated)
		{
			uint duration = 0;

			if (animated)
			{
				duration = 250;
			}

			this.FadeTo(opacity, duration);
		}
	}
}
