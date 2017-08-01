
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

[assembly: ExportRenderer(typeof(AddStyleItem), typeof(AddStyleItemRenderer))]
namespace mobile_style_editor
{
    public class AddStyleItemRenderer : VisualElementRenderer<RelativeLayout>
    {
        AddStyleItem View { get; set; }

        protected override void OnElementChanged(ElementChangedEventArgs<RelativeLayout> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                View = e.NewElement as AddStyleItem;

#if __IOS__
                Layer.BorderWidth = View.BorderWidth;
                Layer.BorderColor = View.BorderColor.ToNativeColor().CGColor;

                if (View.Elevated)
                {
                    var path = UIKit.UIBezierPath.FromRect(Bounds);
                    Layer.MasksToBounds = false;
                    Layer.ShadowColor = Colors.CartoNavy.ToNativeColor().CGColor;
                    Layer.ShadowOffset = new CoreGraphics.CGSize(2.0f, 2.0f);
                    Layer.ShadowOpacity = 0.5f;
                    Layer.ShadowPath = path.CGPath;
                }
#endif
            }

        }

    }
}
