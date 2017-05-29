
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
		public double BaseY { get { return Device.OnPlatform(20, 0, 0); } }

		public new double Height { get { return 50 + BaseY; } }

		BaseView statusbar;

		public Navigationbar()
		{
			BackgroundColor = Colors.CartoNavyLight;

			statusbar = new BaseView { BackgroundColor = Colors.CartoNavyLight };
		}

		public override void LayoutSubviews()
		{
			double statusbarHeight = BaseY;

			double x = 0;
			double y = 0;
			double w = Width;
			double h = statusbarHeight;

			AddSubview(statusbar, x, y, w, h);
		}

		public void Add(BaseView parent, double width)
		{
			parent.AddSubview(parent, 0, BaseY, width, Height);
		}
	}
}
