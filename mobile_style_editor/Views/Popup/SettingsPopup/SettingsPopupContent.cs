
using System;

namespace mobile_style_editor
{
    public class SettingsPopupContent : BasePopupContent
    {
        public UserInfo GithubInfo { get; private set; }

        public UserInfo DriveInfo { get; private set; }

        public BaseView separator1;

        public SettingsPopupContent()
        {
            GithubInfo = new UserInfo();

            DriveInfo = new UserInfo();
            DriveInfo.LogoutButton.IsVisible = false;

            separator1 = new BaseView();
            separator1.BackgroundColor = Colors.CartoNavyTransparent;
        }

        public override void LayoutSubviews()
        {
            double itemWidth = Width;
            double itemHeight = 100;
            double padding = itemHeight / 9;

            double x = 0;
            double y = 0;
            double w = itemWidth;
            double h = itemHeight;

            AddSubview(GithubInfo, x, y, w, h);

            double separatorHeight = 1;
            double separatorPadding = Width / 12;

            y += h + separatorHeight;

            x = separatorPadding;
            w = Width - 2 * separatorPadding;
            h = separatorHeight;

            AddSubview(separator1, x, y, w, h);

            y += h + separatorHeight + padding;

            x = 0;
            w = itemWidth;
            h = itemHeight;

            AddSubview(DriveInfo, x, y, w, h);
        }

    }
}
