using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class ClickView : BaseView
	{
		public EventHandler<EventArgs> Click;

		public ClickView()
		{
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
	}
}
