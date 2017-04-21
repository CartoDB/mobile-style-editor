using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public static class ColorExtensions
	{
#if __IOS__
		public static UIKit.UIColor ToNativeColor(this Color Color)
		{
			return UIKit.UIColor.FromRGBA((nfloat)Color.R, (nfloat)Color.G, (nfloat)Color.B, (nfloat)Color.A);
		}
#elif __ANDROID__
		public static Android.Graphics.Color ToNativeColor(this Color color)
		{
			var native = Android.Graphics.Color.Argb((int)(color.A * 255), (int)(color.R * 255), (int)(color.G * 255), (int)(color.B * 255));
			return native;
		}
#elif __UWP__
        public static Windows.UI.Color ToNativeColor(this Color color)
        {
            return Windows.UI.Color.FromArgb((byte)(color.A * 255), (byte)(color.R * 255), (byte)(color.G * 255), (byte)(color.B * 255));
        }
#endif
	}
}
