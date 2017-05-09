
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class StyleView : BaseView
	{
		public StyleContainer MyStyles { get; private set; }

		public StyleContainer Templates { get; private set; }

		public AddStyleItem AddStyle { get; private set; }

		public StyleView()
		{
			BackgroundColor = Colors.CartoRed;

			MyStyles = new StyleContainer();
			MyStyles.Header = StyleContainer.GetHeaderLabel("MY STYLES");

			Templates = new StyleContainer();
			Templates.Header = StyleContainer.GetHeaderLabel("TEMPLATE STYLES");

			AddStyle = new AddStyleItem();
			MyStyles.Footer = AddStyle;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			double x = 0;
			double y = 0;
			double w = Width / 2;
			double h = Height;

			AddSubview(MyStyles, x, y, w, h);

			x += w;

			AddSubview(Templates, x, y, w, h);
		}

	}
}
