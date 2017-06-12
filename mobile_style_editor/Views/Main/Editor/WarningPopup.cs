
using System;
using Xamarin.Forms;

namespace mobile_style_editor.Views.Main.Editor
{
    public class WarningPopup : BaseView
    {
        public WarningPopup()
        {
            BackgroundColor = Colors.CartoNavyTransparent;
        }

        public override void LayoutSubviews()
        {
            
        }
    }

    public class WarningPopupBox : BaseView
    {
        Label header;
        BaseView separator1;
        Label content;
        BaseView separator2;
        public ClickView Button { get; private set; }

        public WarningPopupBox()
        {
            
        }

        public override void LayoutSubviews()
        {
            
        }
    }

}
