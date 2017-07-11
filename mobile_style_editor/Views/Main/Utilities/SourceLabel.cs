
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class SourceLabel : BaseView
	{
        public EventHandler<EventArgs> Done;

		Label label;
        Label field;

        public string Text { get { return field.Text.Trim(); } set { field.Text = " " + value; } }

		public SourceLabel()
		{
			BackgroundColor = Colors.CartoNavyTransparent;

			label = new Label();
			label.Text = "SOURCE";
			label.VerticalTextAlignment = TextAlignment.Center;
			label.HorizontalTextAlignment = TextAlignment.Center;
			label.FontAttributes = FontAttributes.Bold;
			label.FontSize = 12;
			label.TextColor = Color.White;

			field = new Label();
			field.FontSize = 12;
			field.FontAttributes = FontAttributes.Bold;
			field.BackgroundColor = Colors.CartoNavyTransparent;
            field.VerticalTextAlignment = TextAlignment.Center;
			field.TextColor = Color.White;
            //field.IsEnabled = false;
            //field.Completed += (sender, e) => {
                
            //    if (Done != null)
            //    {
            //        Done(this, e);
            //    }
            //};
		}

		public override void LayoutSubviews()
		{
			double padding = 5;
			double third = (Width - 3 * padding) / 3;

			double x = padding;
			double y = padding;
			double w = third;
			double h = Height - 2 * padding;

			AddSubview(label, x, y, w, h);

			x += w + padding;
			w = 2 * third;

			AddSubview(field, x, y, w, h);
		}
	}
}
