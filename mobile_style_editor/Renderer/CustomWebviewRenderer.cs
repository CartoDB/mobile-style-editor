
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

[assembly: ExportRenderer(typeof(CustomWebView), typeof(CustomWebviewRenderer))]
namespace mobile_style_editor
{
    public class CustomWebviewRenderer : WebViewRenderer
    {
        CustomWebView View { get; set; }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            if (e.NewElement != null)
            {
                View = (CustomWebView)e.NewElement;
                View.OnCookieDelete += DeleteCookies;
            }
        }

        void DeleteCookies(object sender, EventArgs e)
        {
            string domain = (string)sender;

            var cache = Foundation.NSUrlCache.SharedCache;
            cache.RemoveAllCachedResponses();
            cache.DiskCapacity = 0;
            cache.MemoryCapacity = 0;

            EvaluateJavascript("localStorage.clear();");

            var jar = Foundation.NSHttpCookieStorage.SharedStorage;

            foreach (var cookie in jar.Cookies)
            {

                if (cookie.Domain.Contains(domain))
                {
                    jar.DeleteCookie(cookie);
                }
            }
        }

    }
}
