﻿
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
    public class CustomLoader : BaseView
    {
        ActivityIndicator indicator;

        public bool IsRunning
        {
            get { return indicator.IsRunning; }
            set { indicator.IsRunning = value; IsVisible = value; }
        }
        public CustomLoader()
        {
            indicator = new ActivityIndicator();
        }

        public override void LayoutSubviews()
        {
            AddSubview(indicator, 0, 0, Width, Height);
        }
    }
}
