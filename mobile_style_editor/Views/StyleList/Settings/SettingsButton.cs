
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
    public class SettingsButton : ClickView
    {
        Image image;

        public SettingsButton()
        {
            image = new Image();
            image.Source = ImageSource.FromFile("icon_settings.png");
        }

        public override void LayoutSubviews()
        {
            double padding = Height / 6;

            AddSubview(image, padding, padding, Width - 2 * padding, Height - 2 * padding);
        }
    }
}
