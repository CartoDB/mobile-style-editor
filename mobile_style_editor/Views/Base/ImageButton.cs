﻿
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
    public class ImageButton : ClickView
    {
		Image image;

        double padding;
        public double ImagePadding
        {
            get { return padding; } 
            set { padding = value; }
        }

        public string Source 
        {
            get { return image.Source.StyleId; } 
            set { image.Source = ImageSource.FromFile(value); }
        }

        public ImageButton()
        {
            image = new Image();
        }

        public override void LayoutSubviews()
        {
            double x = padding;
            double y = padding;
            double h = Height - 2 * padding;
            double w = Width - 2 * padding;

            AddSubview(image, x, y, w, h);
        }
    }
}
