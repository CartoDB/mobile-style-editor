
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class NavigationBar : BaseView
	{
        public static readonly double HEIGHT = 50;

		public bool IsBackButtonVisible { get; set; } = true;

		public double BaseY { get { return Device.OnPlatform(20, 0, 0); } }

		public new double Height { get { return HEIGHT + BaseY; } }

		BaseView statusbar, container;

		public NavigationBackButton Back { get; private set; }

		public Label Title { get; private set; }

        BaseView rightItem;

		public NavigationBar()
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

			if (IsBackButtonVisible)
			{
                w = h * 2;
				container.AddSubview(Back, x, y, w, h);
            } else
            {
                w = h;    
            }


			x = w;
			y = BaseY;
			w = Width - 2 * w;
			h = Height - statusbarHeight;

			AddSubview(Title, x, y, w, h);

            if (rightItem != null) 
            {
                h = Height - statusbarHeight;
                w = h;
                x = Width - w;
                y = BaseY;

                AddSubview(rightItem, x, y, w, h);   
            }
		}

		public void Add(BaseView parent, double width)
		{
			parent.AddSubview(parent, 0, BaseY, width, Height);
		}

        public void AddRightBarButton(BaseView view)
        {
            rightItem = view;
        }
	}

}
