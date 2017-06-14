﻿using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
    public class BasePopup : BaseView
    {
        public EventHandler<EventArgs> Click;

        public BasePopupContent Content { get; protected set; }

        public PopupHeader Header { get; private set; }

        public ImageButton CloseButton { get; set; }

        public SelectButton Select { get; private set; }

		protected double VerticalPadding 
        { 
            get 
            {
                if (DeviceInfo.IsSmallScreen)
                {
                    return 60;
                }

                return 70;
            }
        }

		protected double HorizontalPadding
		{
			get
			{
                if (DeviceInfo.IsSmallScreen)
                {
                    return 5;
                }

				if (Width > Height)
				{
					return Width / 6;
				}
				
				return Width / 15;
			}
		}

        protected double ContentX { get { return HorizontalPadding; } }

        protected double ContentY { get { return VerticalPadding; } }

		protected double ContentWidth { get { return Width - 2 * HorizontalPadding; } }

		protected double ContentHeight { get { return Height - 2 * VerticalPadding; } }

        double padding { get { return 10;  } }

        protected double HeaderHeight { get { return NavigationBar.HEIGHT; } }

        protected double HeaderY { get { return VerticalPadding - HeaderHeight; } }

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

			Header = new PopupHeader();

            CloseButton = new ImageButton();
            CloseButton.BackgroundColor = Colors.CartoRed;
            CloseButton.Source = "icon_close.png";

            CloseButton.Click += OnBackgroundClick;

            Select = new SelectButton();

		}

        public override void LayoutSubviews()
        {
			double x = ContentX;
			double y = ContentY;
			double w = ContentWidth;
			double h = ContentHeight;

			AddSubview(Content, x, y, w, h);

			w = ContentWidth;
			h = HeaderHeight;
			x = HorizontalPadding;
			y = VerticalPadding - h;

			AddSubview(Header, x, y, w, h);

            if (!DeviceInfo.IsSmallScreen)
            {
				int halfWidth = 20;

				w = 2 * halfWidth;
				h = w;
				x = ContentX + ContentWidth - halfWidth;
				y = HeaderY - halfWidth;

				CloseButton.ImagePadding = halfWidth / 3;

                AddSubview(CloseButton, x, y, w, h);
                CloseButton.CornerRadius = halfWidth;
            }
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
