using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace mobile_style_editor
{
    public class ZoomControl : BaseView
    {
        public ZoomButton In { get; private set; }
        public ZoomButton Out { get; private set; }

        public ZoomControl()
        {
            BackgroundColor = Colors.CartoNavyTransparent;

            In = new ZoomButton("Assets/icon_zoom_in.png");
            In.BackgroundColor = Colors.CartoNavy;
            Out = new ZoomButton("Assets/icon_zoom_out.png");
            Out.BackgroundColor = Colors.CartoNavy;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            double padding = 5;

            double x = padding;
            double y = padding;
            double w = (Width - 3 * padding) / 2;
            double h = Height - 2 * padding;

            AddSubview(In, x, y, w, h);

            x += w + padding;

            AddSubview(Out, x, y, w, h);
        }
    }

    public class ZoomButton : ClickView
    {
        Image image;

        public ZoomButton(string asset)
        {
            image = new Image();
            image.Source = ImageSource.FromFile(asset);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            double padding = 5;

            double x = Width / 2 - Height / 2 + padding;
            double y = padding;
            double w = Height - 2 * padding;
            double h = w;

            AddSubview(image, x, y, w, h);
        }
    }

}
