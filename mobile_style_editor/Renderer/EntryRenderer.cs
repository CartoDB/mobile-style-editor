
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

//[assembly: ExportRenderer(typeof(BaseEntry), typeof(mobile_style_editor.EntryRenderer))]
//namespace mobile_style_editor
//{
//    public class EntryRenderer : ViewRenderer
//    {
//        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
//        {
//            base.OnElementChanged(e);

//#if __ANDROID__
//            //SetPadding(0, 0, 0, 0);
//#endif
//        }

//    }
//}
