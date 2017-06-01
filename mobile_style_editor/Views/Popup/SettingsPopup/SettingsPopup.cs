
using System;

namespace mobile_style_editor
{
    public class SettingsPopup : BasePopup
    {
        public SettingsPopupContent SettingsContent { get { return Content as SettingsPopupContent; } }

		public SettingsPopup()
		{
            Content = new SettingsPopupContent();
			
            Header.HideButtons();

            Header.Text = "SETTINGS";
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
        }
    }
}
