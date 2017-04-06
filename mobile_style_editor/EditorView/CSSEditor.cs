
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
				SetTextColor(textColor.ToNativeColor());
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
			BackgroundColor = Color.FromRgb(50, 50, 50);
			TextColor = Color.White;

			TextSize = 13f;

			rect = new Android.Graphics.Rect();
			paint = new Android.Graphics.Paint();

			float paintSize = 26f;

			paint.SetStyle(Android.Graphics.Paint.Style.Fill);
			paint.Color = Android.Graphics.Color.Gray;
			paint.TextSize = paintSize;

			SetPadding(2 * (int)paintSize, 0, 0, 0);
		}

		Android.Graphics.Rect rect;
		Android.Graphics.Paint paint;

		override protected void OnDraw(Android.Graphics.Canvas canvas)
		{
			int baseline = Baseline;

			for (int i = 0; i < LineCount; i++)
			{
				int number = i + 1;
				string text =  " ";

				if (number < 10)
				{
					text += "0";
				}

				text += "" + (i + 1);

				canvas.DrawText(text, rect.Left, baseline, paint);

				baseline += LineHeight;
			}
			base.OnDraw(canvas);
		}

		public void Update(string text)
		{
			Text = text;
		}
	}
}
