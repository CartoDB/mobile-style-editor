
using System;

namespace mobile_style_editor
{
    public class DeviceInfo
    {
        public static bool IsSmallScreen
        {
            get
            {
#if __ANDROID__
                var fHD = 1920 * 1080;

                var context = Xamarin.Forms.Forms.Context;

                var width = context.Resources.DisplayMetrics.WidthPixels;
                var height = context.Resources.DisplayMetrics.HeightPixels;

                var density = context.Resources.DisplayMetrics.Density;

                if (height * width < fHD)
                {
                    return true;
                }

                float widthDp = width / density;
                float heightDp = height / density;

                float smallest = Math.Min(widthDp, heightDp);

                int inch7 = 600;
                int inch10 = 720;

                if (widthDp > height)
                {
                    // Let's support editor on 7 inch tablets that are in landscape,
                    if (smallest <= inch7)
                    {
                        return true;
                    }
                }
                else
                {
                    // But portrait editor only supported for 10 inch tablets
                    if (smallest <= inch10)
                    {
                        return true;
                    }
                }

                return false;
#elif __IOS__
                return UIKit.UIDevice.CurrentDevice.UserInterfaceIdiom != UIKit.UIUserInterfaceIdiom.Pad;
#elif __UWP__
                return false;
#endif
            }
        }
    }
}
