using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
    public class BasePopup : BaseView
    {
        public EventHandler<EventArgs> Click;

        public BasePopupContent Content { get; protected set; }

        public FileListHeader Header { get; private set; }

		protected double VerticalPadding { get { return 70; } }

		protected double HorizontalPadding
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

        protected double ContentX { get { return HorizontalPadding; } }

        protected double ContentY { get { return VerticalPadding; } }

		protected double ContentWidth { get { return Width - 2 * HorizontalPadding; } }

		protected double ContentHeight { get { return Height - 2 * VerticalPadding; } }

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

			Header = new FileListHeader();
		}

        public override void LayoutSubviews()
        {
			double x = ContentX;
			double y = ContentY;
			double w = ContentWidth;
			double h = ContentHeight;

			AddSubview(Content, x, y, w, h);

			double padding = 10;

			w = ContentWidth;
			h = VerticalPadding - 3 * padding;
			x = HorizontalPadding;
			y = VerticalPadding - h;

			AddSubview(Header, x, y, w, h);
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

        public bool Toggle()
        {
            if (IsVisible)
            {
                Hide();
                return false;
            }
            else
            {
                Show();
                return true;
            }
        }
    }
}
