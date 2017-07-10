
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
    public class CustomWebView : WebView
    {
        public EventHandler<EventArgs> OnCookieDelete;

        public CustomWebView()
        {
        }

        public void DeleteCookies(string domain)
        {
            if (OnCookieDelete != null) {
                OnCookieDelete(domain, EventArgs.Empty);
            }


        }
    }
}
