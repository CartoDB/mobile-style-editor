
using System;
using mobile_style_editor;
using Xamarin.Forms;


#if __IOS__
using Xamarin.Forms.Platform.iOS;
#elif __ANDROID__
using Android.Webkit;
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

#if __ANDROID__
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                View = (CustomWebView)e.NewElement;
                View.OnCookieDelete += DeleteCookies;
            }
        }
#elif __IOS__
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

			if (e.NewElement != null)
			{
				View = (CustomWebView)e.NewElement;
				View.OnCookieDelete += DeleteCookies;
			}
        }
#endif

        void DeleteCookies(object sender, EventArgs e)
        {
            string domain = (string)sender;

            // TODO Don't remove all cookies or cache, but domain-specific
#if __ANDROID__

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                CookieManager.Instance.RemoveAllCookies(null);
                CookieManager.Instance.Flush();
            }
            else
            {
                CookieSyncManager manager = CookieSyncManager.CreateInstance(Forms.Context);
                manager.StartSync();

                CookieManager.Instance.RemoveAllCookie();
                CookieManager.Instance.RemoveSessionCookie();

                manager.StopSync();
                manager.Sync();
            }

#elif __IOS__
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
#endif
        }

    }
}
