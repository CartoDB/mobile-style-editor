using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class BasePopup : BaseView
	{
		public EventHandler<EventArgs> Click;

		public BasePopupContent Content { get; protected set; }

		public BasePopup()
		{
			BackgroundColor = Colors.TransparentGray;

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
			Hide();
		}

		public void Show(bool animated = true)
		{
			if (!animated)
			{
				this.FadeTo(1, 0);
			}
			else
			{
				this.FadeTo(1);
			}
		}

		public void Hide(bool animated = true)
		{
			if (!animated)
			{
				this.FadeTo(0, 0);
			}
			else
			{
				this.FadeTo(0);
			}
		}
	}
}
