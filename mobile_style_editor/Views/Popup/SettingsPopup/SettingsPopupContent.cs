
using System;

namespace mobile_style_editor
{
    public class SettingsPopupContent : BasePopupContent
    {
        public UserInfo UserInfo { get; private set; }

        public BaseView separator1;

        public SettingsPopupContent()
        {
            UserInfo = new UserInfo();

            separator1 = new BaseView();
            separator1.BackgroundColor = Colors.CartoNavyTransparent;
        }

        public override void LayoutSubviews()
        {
            double x = 0;
            double y = 0;
            double w = Width;
            double h = 100;

            AddSubview(UserInfo, x, y, w, h);

            double separatorHeight = 1;
            double separatorPadding = Width / 12;

            y += h + separatorHeight;

            x = separatorPadding;
            w = Width - 2 * separatorPadding;
            h = separatorHeight;

            AddSubview(separator1, x, y, w, h);
        }
    }
}
