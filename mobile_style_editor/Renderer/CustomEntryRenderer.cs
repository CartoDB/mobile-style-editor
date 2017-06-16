
using System;
using mobile_style_editor;
using Xamarin.Forms;

#if __IOS__
using Xamarin.Forms.Platform.iOS;
#elif __ANDROID__
using Xamarin.Forms.Platform.Android;
#elif __UWP__
using Xamarin.Forms.Platform.UWP;
#endif

[assembly: ExportRenderer(typeof(BaseEntry), typeof(CustomEntryRenderer))]
namespace mobile_style_editor
{
    public class CustomEntryRenderer : EntryRenderer

    {
        BaseEntry View;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

#if __ANDROID__
            if (Control != null)
            {
                Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
                Control.SetPadding(5, 0, 0, 0);
                Control.Gravity = Android.Views.GravityFlags.CenterVertical;
            }
#elif __IOS__
            if (e.NewElement != null)
            {
                View = (BaseEntry)e.NewElement;

                if (View.AutoCorrectEnabled)
                {
                    EnableAutoCorrect();
                }
                else
                {
                    DisableAutoCorrect();
                }
            }
#endif
        }

        void DisableAutoCorrect()
        {
#if __IOS__
            Control.AutocorrectionType = UIKit.UITextAutocorrectionType.No;
#elif __ANDROID__
#endif
        }

        void EnableAutoCorrect()
        {
#if __IOS__
            Control.AutocorrectionType = UIKit.UITextAutocorrectionType.Yes;
#elif __ANDROID__
#endif
        }

    }
}
