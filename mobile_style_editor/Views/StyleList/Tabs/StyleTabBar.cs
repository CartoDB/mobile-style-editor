using System;
namespace mobile_style_editor
{
	public class StyleTabBar : BaseView
	{
		public StyleTab MyStyles { get; private set; }

		public StyleTab Templates { get; private set; }

		public StyleIndicator Indicator { get; private set; }

		public StyleTabBar()
		{
			MyStyles = new StyleTab();
			MyStyles.Text = "MY STYLES";

			Templates = new StyleTab();
			Templates.Text = "TEMPLATES";

			Indicator = new StyleIndicator();
		}

		public override void LayoutSubviews()
		{
			double x = 0;
			double y = 0;
			double w = (Width) / 2;
			double h = (Height) / 2;

			AddSubview(MyStyles, x, y, w, h);

			x += h;

			AddSubview(Templates, x, y, w, h);

			double indicatorSize = Height / 10;

			h = indicatorSize;
			y = Height - h;

			AddSubview(Indicator, x, y, w, h);
		}
	}

	public class StyleIndicator : BaseView
	{
		public StyleIndicator()
		{
			BackgroundColor = Colors.CartoPurple;
		}

		public void MoveTo(double x)
		{
			UpdateLayout(x, Y, Width, Height);
		}
	}
}
