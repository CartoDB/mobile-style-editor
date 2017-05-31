
using System;

namespace mobile_style_editor
{
    public class SettingsPopupContent : BasePopupContent
    {
        public UserInfo UserInfo { get; private set; }

        public SettingsPopupContent()
        {
            UserInfo = new UserInfo();
        }

        public override void LayoutSubviews()
        {
            double x = 0;
            double y = 0;
            double w = Width;
            double h = 100;

            AddSubview(UserInfo, x, y, w, h);
        }
    }
}
