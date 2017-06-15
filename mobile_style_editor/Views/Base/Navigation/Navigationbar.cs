
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class NavigationBar : BaseView
	{
        public static readonly double HEIGHT = 50;

		public bool IsBackButtonVisible { get; set; } = true;

        public bool IsTitleEditVisible { get; set; }

        public double BaseY
        {
            get
            {
                if (Device.RuntimePlatform.Equals("iOS")) { return 20; }
                return 0;
            }
        }

		public new double Height { get { return HEIGHT + BaseY; } }

		BaseView statusbar, container;

		public NavigationBackButton Back { get; private set; }

        public TitleEditButton Edit { get; private set; }

        public MeasuredLabel Title { get; private set; }

        BaseView rightItem;

		public NavigationBar()
		{
            Console.WriteLine(Device.RuntimePlatform);

			BackgroundColor = Colors.CartoNavyLight;

			statusbar = new BaseView { BackgroundColor = Colors.CartoNavyLight };
			container = new BaseView();

			Title = new MeasuredLabel();
			Title.TextColor = Color.White;
			Title.FontAttributes = FontAttributes.Bold;
			Title.VerticalTextAlignment = TextAlignment.Center;
			Title.HorizontalTextAlignment = TextAlignment.Center;
			Title.FontSize = 14;

			Back = new NavigationBackButton();

            Edit = new TitleEditButton();

			Title.Measured += delegate
			{
                double x = Width / 2 - Title.MeasuredWidth / 2;

                Title.UpdateLayout(x, BaseY, Title.MeasuredWidth, Height - BaseY);
                Edit.UpdateX(x + Title.MeasuredWidth);
			};
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
            w = Title.MeasuredWidth;
			h = Height - statusbarHeight;

			AddSubview(Title, x, y, w, h);

            if (IsTitleEditVisible)
            {
                x = Title.X + Title.Width;
                w = Height / 2;
                h = w;

                AddSubview(Edit, x, y, w, h);
            }

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
