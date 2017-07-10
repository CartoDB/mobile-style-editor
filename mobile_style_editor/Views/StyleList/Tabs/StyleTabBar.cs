using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class StyleTabBar : BaseView
	{
		public EventHandler<EventArgs> TabClicked;

		public StyleTab MyStyles { get; private set; }

		public StyleTab Templates { get; private set; }

		public StyleIndicator Indicator { get; private set; }

		public StyleTabBar()
		{
			BackgroundColor = Color.White;

			MyStyles = new StyleTab();
			MyStyles.Text = "MY STYLES";
			MyStyles.Click += OnTabClick;

			Templates = new StyleTab();
			Templates.Text = "TEMPLATES";
			Templates.Click += OnTabClick;

			Indicator = new StyleIndicator();
		}

		public override void LayoutSubviews()
		{
			double x = 0;
			double y = 0;
			double w = Width / 2;
			double h = Height;

			AddSubview(MyStyles, x, y, w, h);

			x += w;

			AddSubview(Templates, x, y, w, h);

			double indicatorSize = Height / 10;

			h = indicatorSize;

			x = 0;
			y = Height - h;

			AddSubview(Indicator, x, y, w, h);
			Indicator.TranslationX = 0;
		}

		void OnTabClick(object sender, EventArgs e)
		{
			if (TabClicked != null)
			{
				StyleTab tab = (StyleTab)sender;

				Indicator.MoveTo(tab.X);

				TabClicked(sender, e);
			}
		}

        public void ScrollToMyStyles()
        {
            ScrollTo(MyStyles);
        }

        public void ScrollToTemplates()
        {
            ScrollTo(Templates);
        }

        public void ScrollTo(StyleTab tab)
        {
            Device.BeginInvokeOnMainThread(delegate {
               Indicator.MoveTo(tab.X); 
            });
        }
	}

	public class StyleIndicator : BaseView
	{
		public StyleIndicator()
		{
            BackgroundColor = Colors.CartoNavy;
		}

		public void MoveTo(double x)
		{
			//UpdateLayout(x, Y, Width, Height);
			this.TranslateTo(x, 0);
		}
	}
}
