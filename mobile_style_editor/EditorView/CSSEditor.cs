
using System;
using Xamarin.Forms;

#if __IOS__
using Xamarin.Forms.Platform.iOS;
#elif __ANDROID__
using Xamarin.Forms.Platform.Android;
#endif

namespace mobile_style_editor
{
#if __IOS__
	public class CSSEditor : UIKit.UITextField
#elif __ANDROID__
	public class CSSEditor : Android.Widget.EditText
#endif
	{
		Color textColor;
		public Color TextColor
		{
			get { return textColor; }
			set
			{
				textColor = value;
#if __IOS__
				base.TextColor = backgroundColor.ToNativeColor();
#elif __ANDROID__
				SetTextColor(backgroundColor.ToNativeColor());
#endif
			}
		}

		Color backgroundColor;
		public Color BackgroundColor
		{
			get { return backgroundColor; }
			set
			{
				backgroundColor = value;
#if __IOS__
				base.BackgroundColor = backgroundColor.ToNativeColor();
#elif __ANDROID__
				SetBackgroundColor(backgroundColor.ToNativeColor());
#endif
			}
		}

		public CSSEditor() 
#if __ANDROID__ 
		: base(Forms.Context)
#endif
		{
			BackgroundColor = Color.Black;
			TextColor = Color.White;
		}
	}
}
