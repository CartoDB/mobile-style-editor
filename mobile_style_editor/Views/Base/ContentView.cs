﻿﻿
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
    public class ContentView : BaseView
    {
        public Navigationbar Navigationbar { get; private set; }

        BaseView container;

		public new double Width { get { return base.Width; } }

		public new double Height
		{
			get
			{
				if (IsNavigationBarVisible)
				{
					return base.Height - Navigationbar.Height;
				}

				return base.Height;
			}
		}

        public bool IsNavigationBarVisible
        {
            get { return Navigationbar != null; }
            set
            {
                if (value)
                {
                    Navigationbar = new Navigationbar();
                }
                else
                {
                    Navigationbar = null;
                }
            }
        }

        public ContentView()
        {
            container = new BaseView();
        }

        public override void LayoutSubviews()
        {
            double x = 0;
            double y = 0;
            double w = Width;
            double h = base.Height;

            if (IsNavigationBarVisible)
            {
                h = Navigationbar.Height;
                base.AddSubview(Navigationbar, x, y, w, h);

                y += h;
				h = base.Height - Navigationbar.Height;
            }

            base.AddSubview(container, x, y, w, h);
        }

        public override void AddSubview(View view)
        {
            if (IsNavigationBarVisible)
            {
                container.AddSubview(view);
            }
            else
            {
                base.AddSubview(view);
            }
        }

        public override void AddSubview(View view, double x, double y, double w, double h)
        {
            if (IsNavigationBarVisible)
            {
                container.AddSubview(view, x, y, w, h);
            }
            else
            {
                base.AddSubview(view, x, y, w, h);
            }
        }

        public override void RemoveChild(View view)
        {
            if (IsNavigationBarVisible)
            {
                container.RemoveChild(view);
            }
            else
            {
                base.RemoveChild(view);
            }
        }
    }

	public class Navigationbar : BaseView
	{
        public bool IsBackButtonVisible { get; set; } = true;

		public double BaseY { get { return Device.OnPlatform(20, 0, 0); } }

		public new double Height { get { return 50 + BaseY; } }

		BaseView statusbar, container;

		public NavigationBackButton Back { get; private set; }

		public Label Title { get; private set; }

		public Navigationbar()
		{
			BackgroundColor = Colors.CartoNavyLight;

			statusbar = new BaseView { BackgroundColor = Colors.CartoNavyLight };
			container = new BaseView();

			Title = new Label();
			Title.TextColor = Color.White;
			Title.FontAttributes = FontAttributes.Bold;
			Title.VerticalTextAlignment = TextAlignment.Center;
			Title.HorizontalTextAlignment = TextAlignment.Center;
			Title.FontSize = 14;

			Back = new NavigationBackButton();
		}

		public override void LayoutSubviews()
		{
			double statusbarHeight = BaseY;

			double x = 0;
			double y = 0;
			double w = Width;
			double h = statusbarHeight;

			AddSubview(statusbar, x, y, w, h);

			y += h;
			h = Height - statusbarHeight;

			AddSubview(container, x, y, w, h);

			x = 0;
			y = 0;
            w = h * 3;

            if (IsBackButtonVisible)
            {
                container.AddSubview(Back, x, y, w, h);
            }


            x = w;
            y = BaseY;
            w = Width - 2 * w;
            h = Height - statusbarHeight;

            AddSubview(Title, x, y, w, h);
		}

		public void Add(BaseView parent, double width)
		{
			parent.AddSubview(parent, 0, BaseY, width, Height);
		}
	}

	public class NavigationBackButton : ClickView
	{
		Image image;
		Label label;

		public NavigationBackButton()
		{
			label = new Label();
			label.TextColor = Color.White;
			label.Text = "BACK";
			label.FontAttributes = FontAttributes.Bold;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.FontSize = 14;

			image = new Image();

			string folder = "";
#if __UWP__
			folder = "Assets/";
#endif
			image.Source = ImageSource.FromFile(folder + "icon_arrow_back.png");
		}

		public override void LayoutSubviews()
		{
			double imagePadding = Height / 5;
            double labelPadding = 0;

			double x = imagePadding;
			double y = imagePadding;
			double h = Height - 2 * imagePadding;
			double w = h;

			AddSubview(image, x, y, w, h);

            y = labelPadding;
            x += w + imagePadding;
            w = Width - (w + 2 * imagePadding);
            h = Height;

			AddSubview(label, x, y, w, h);
		}
	}
}
