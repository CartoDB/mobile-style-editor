
using System;
using mobile_style_editor;
using Xamarin.Forms;

#if __IOS__
using Xamarin.Forms.Platform.iOS;
#elif __ANDROID__
using Android.Views;
using Xamarin.Forms.Platform.Android;
#elif __UWP__
using Xamarin.Forms.Platform.UWP;
#endif

[assembly: ExportRenderer(typeof(BaseScrollView), typeof(CustomScrollViewRenderer))]
namespace mobile_style_editor
{
    public class CustomScrollViewRenderer : ScrollViewRenderer
    {
        BaseScrollView View { get; set; }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                View = (BaseScrollView)e.NewElement;
#if __IOS__
                ScrollEnabled = View.ScrollEnabled;
#endif
            }
        }

#if __ANDROID__

#endif
    }
}
