
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
			double x = contentX;
			double y = contentY;
			double w = contentWidth;
			double h = contentHeight;

			AddSubview(Content, x, y, w, h);
        }
    }
}
