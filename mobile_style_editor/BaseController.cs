
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
    public class BaseController : ContentPage
    {
		/// <summary>
        /// Convenience method of Xamarin.Form's await DisplayAlert(); 
		///  If "ok" is null, "cancel" functions as ok()
		/// </summary>
		public async void Alert(string title, string message, Action cancel, Action ok = null)
        {
            bool success = false;

            if (ok != null) {
                success = await DisplayAlert(title, message, "Ok", "Cancel");
                if (success) {
                    ok();
                } else
                {
                    if (cancel != null)
                    {
                        cancel();
                    }
                }
            } 
            else
            {
                await DisplayAlert(title, message, "Ok");
                if (cancel != null)
                {
                    cancel();
                }
            }

        }
    }
}
