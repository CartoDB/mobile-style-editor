﻿﻿
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
    public class ContentView : BaseView
    {
        public NavigationBar NavigationBar { get; private set; }

        BaseView container;

        public new double Width { get { return base.Width; } }

		public new double Height
		{
			get
			{
				if (IsNavigationBarVisible)
				{
					return base.Height - NavigationBar.Height;
				}

				return base.Height;
			}
		}

        public bool IsNavigationBarVisible
        {
            get { return NavigationBar != null; }
            set
            {
                if (value)
                {
                    NavigationBar = new NavigationBar();
                }
                else
                {
                    NavigationBar = null;
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
                h = NavigationBar.Height;
                base.AddSubview(NavigationBar, x, y, w, h);

                y += h;
				h = base.Height - NavigationBar.Height;
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
}
