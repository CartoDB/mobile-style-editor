using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
    public class BasePopup : BaseView
    {
        public EventHandler<EventArgs> Click;

        public BasePopupContent Content { get; protected set; }


		protected double verticalPadding { get { return 70; } }
		protected double horizontalPadding
		{
			get
			{
				if (Width > Height)
				{
					return Width / 6;
				}
				else
				{
					return Width / 15;
				}

			}
		}

        protected double contentX { get { return horizontalPadding; } }
        protected double contentY { get { return verticalPadding; } }
		protected double contentWidth { get { return Width - 2 * horizontalPadding; } }
		protected double contentHeight { get { return Height - 2 * verticalPadding; } }

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

            Hide(false);
        }

        void OnBackgroundClick(object sender, EventArgs e)
        {
            Hide();
        }

        public void Show(bool animated = true)
        {
            IsVisible = true;

            if (!animated)
            {
                Opacity = 1;
            }
            else
            {
                this.FadeTo(1);
            }
        }

        public virtual async void Hide(bool animated = true)
        {
            if (!animated)
            {
                Opacity = 0;
            }
            else
            {
                await this.FadeTo(0);
            }

            IsVisible = false;
        }

        public void Toggle()
        {
            if (IsVisible)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }
}
