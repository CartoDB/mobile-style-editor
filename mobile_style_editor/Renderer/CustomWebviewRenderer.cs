
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

#if __ANDROID__
            CookieManager.Instance.SetCookie(domain, "");
            string cookie = CookieManager.Instance.GetCookie(domain);

            Console.WriteLine("Cookie: " + cookie);
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
