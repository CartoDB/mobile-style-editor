
namespace mobile_style_editor
{
	public class Toast
	{
		public static void Show(string text)
		{
#if __ANDROID__
			Android.Widget.Toast.MakeText(Xamarin.Forms.Forms.Context, text, Android.Widget.ToastLength.Short).Show();
#elif __IOS__
			ToastIOS.Toast.MakeText(text).SetFontSize(12f).Show();
#elif __UWP__
#endif
		}
	}
}
