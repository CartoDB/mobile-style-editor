using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class BasePopupContent : BaseScrollView
	{
		public EventHandler<EventArgs> Click;

		public BasePopupContent()
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
			// Do nothing. Just here to catch click	
		}

	}
}
