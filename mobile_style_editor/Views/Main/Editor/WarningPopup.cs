
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
    public class WarningPopup : BaseView
    {
        public WarningPopupBox Box { get; private set; }

        public WarningPopup()
        {
            BackgroundColor = Colors.CartoNavyTransparent;

            Box = new WarningPopupBox();
        }

        public override void LayoutSubviews()
        {
            double padding = Width / 10;

            double w = Width - 2 * padding;
            double h = Height / 4;
            double x = padding;
            double y = Height / 2 - h / 2;

            AddSubview(Box, x, y, w, h);
        }
    }

    public class WarningPopupBox : BaseView
    {
        Label header;
        BaseView separator1;
        Label content;
        BaseView separator2;
        public ClickLabel Button { get; private set; }

        public WarningPopupBox()
		{
			//CornerRadius = 10;

			BackgroundColor = Color.White;

            header = new Label();
            this.IsClippedToBounds = true;
            header.VerticalTextAlignment = TextAlignment.Center;
            header.HorizontalTextAlignment = TextAlignment.Center;
            header.FontSize = 14;
            header.FontAttributes = FontAttributes.Bold;
            separator1 = new BaseView();
            separator1.BackgroundColor = Color.LightGray;

            content = new Label();
            content.VerticalTextAlignment = TextAlignment.Center;
            content.HorizontalTextAlignment = TextAlignment.Center;
            content.FontSize = 13;

            separator2 = new BaseView();
            separator2.BackgroundColor = Color.LightGray;

            Button = new ClickLabel();
            Button.Text.TextColor = Colors.CartoNavy;
            Button.BackgroundColor = Colors.NearWhite;
            Button.Text.FontAttributes = FontAttributes.Bold;
            Button.Text.FontSize = 15;

            header.Text = "WARNING!";

            content.Text = "STYLE EDITOR IS AN EXPERIMENTAL FEATURE. PROCEED WITH CAUTION";

            Button.Text.Text = "I UNDERSTAND";
        }

        public override void LayoutSubviews()
        {
            double padding = 10;

            double separatorHeight = 1;
            double headerHeight = Height / 5;
            double buttonHeight = Height / 4;
            double contentHeight = Height - (2 * separatorHeight + headerHeight + buttonHeight);

            double x = padding;
            double y = 0;
            double w = Width - 2 * padding;
            double h = headerHeight;

            AddSubview(header, x, y, w, h);

            y += h;
            h = separatorHeight;

            AddSubview(separator1, x, y, w, h);

            y += h;
            h = contentHeight;

            AddSubview(content, x, y, w, h);

            y += h;
            h = separatorHeight;

            AddSubview(separator2, x, y, w, h);

            y += h;
            x = 0;
            w = Width;
            h = buttonHeight;
			
			AddSubview(Button, x, y, w, h);
		}
    }
}
