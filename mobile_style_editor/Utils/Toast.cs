
namespace mobile_style_editor
{
	public class Toast
	{
#if __ANDROID__
		static Android.Widget.Toast toast;
#endif
		public static void Show(string text)
		{
#if __ANDROID__

			if (toast != null)
			{
				toast.Cancel();
			}

			toast = Android.Widget.Toast.MakeText(Xamarin.Forms.Forms.Context, text, Android.Widget.ToastLength.Short);
			toast.Show();
#elif __IOS__
			ToastIOS.Toast.MakeText(text).SetFontSize(12f).Show();
#elif __UWP__
             
#endif
        }
	}
}
