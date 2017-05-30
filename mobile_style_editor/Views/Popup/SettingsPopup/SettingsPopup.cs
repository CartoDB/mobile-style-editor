
using System;

namespace mobile_style_editor
{
    public class SettingsPopup : BasePopup
    {
        public SettingsPopupContent SettingsContent { get { return Content as SettingsPopupContent; } }

		public SettingsPopup()
		{
            Content = new SettingsPopupContent();
			base.Hide(false);
        }

        public override void LayoutSubviews()
        {
			double x = ContentX;
			double y = ContentY;
			double w = ContentWidth;
			double h = ContentHeight;

			AddSubview(Content, x, y, w, h);
        }
    }
}
