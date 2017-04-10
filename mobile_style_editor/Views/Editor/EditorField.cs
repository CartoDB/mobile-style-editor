
using System;
using Xamarin.Forms;

#if __IOS__
using Xamarin.Forms.Platform.iOS;
using UIKit;
#elif __ANDROID__
using Xamarin.Forms.Platform.Android;
#endif

namespace mobile_style_editor
{
#if __IOS__
	public class EditorField : UITextView
#elif __ANDROID__
	public class EditorField : Android.Widget.EditText
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

		public EditorField()
#if __ANDROID__
		: base(Forms.Context)
#endif
		{
			BackgroundColor = Color.FromRgb(50, 50, 50);
			TextColor = Color.White;

#if __ANDROID__
			TextSize = 13f;

			rect = new Android.Graphics.Rect();
			paint = new Android.Graphics.Paint();

			float paintSize = 26f;

			paint.SetStyle(Android.Graphics.Paint.Style.Fill);
			paint.Color = Android.Graphics.Color.Gray;
			paint.TextSize = paintSize;

			SetPadding(2 * (int)paintSize, 0, 0, 0);
#elif __IOS__
#endif
		}
#if __ANDROID__
		Android.Graphics.Rect rect;
		Android.Graphics.Paint paint;

		override protected void OnDraw(Android.Graphics.Canvas canvas)
		{
			int baseline = Baseline;

			for (int i = 0; i < LineCount; i++)
			{
				int number = i + 1;
				string text = " ";

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

#elif __IOS__
		public override void Draw(CoreGraphics.CGRect rect)
		{
			Console.WriteLine(ContentSize.Height);

			CoreGraphics.CGContext context = UIGraphics.GetCurrentContext();

			foreach (UIView view in Subviews)
			{
				var type = view.GetType();
				Console.WriteLine(type);
			}

			if (Font == null)
			{
				base.Draw(rect);
				return;
			}

			nfloat lineHeight = Font.LineHeight;
			nfloat lineCount = (ContentSize.Height - (ContentInset.Top + ContentInset.Bottom)) / lineHeight;
			                    
			for (int i = 0; i < lineCount; i++) {
				//context.SaveState();

				string text = i + ".";
				var font = UIFont.SystemFontOfSize(12.0f);
				var textRect = new CoreGraphics.CGPoint(1.0f, lineHeight * i);

				text.DrawString(textRect, font);

				//context.RestoreState();
			}

			UIGraphics.EndImageContext();

			base.Draw(rect);
		}
#endif
		public void Update(string text)
		{
			var watch = new System.Diagnostics.Stopwatch();
			watch.Start();

			string[] lines = text.Split('\n');

#if __ANDROID__
			var builder = new Droid.SimpleSpanBuilder();
#elif __IOS__
			var builder = new iOS.AttributedTextBuilder();
#endif
			float size = 1f;

			// White
			Color generalColor = Color.White;
			// Light gray
			Color commentColor = Color.FromRgb(120, 120, 120);
			// Carto green
			Color blockHeaderColor = Color.FromRgb(145, 198, 112);

			foreach (string line in lines)
			{
				string trimmed = line.Trim();
				string withNewLine = line + "\n";

				if (trimmed.StartsWith("//", StringComparison.Ordinal))
				{
					builder.Append(withNewLine, commentColor.ToNativeColor(), size);
				}
				else
				{
					if (trimmed.Contains("#") || trimmed.Contains("["))
					{
						if (trimmed.Contains("{"))
						{
							int bracketIndex = line.IndexOf("{", StringComparison.Ordinal);
							string blockHeader = line.Substring(0, bracketIndex);
							string remaining = line.Substring(bracketIndex, line.Length - bracketIndex);

							builder.Append(blockHeader, blockHeaderColor.ToNativeColor(), size);
							builder.Append(remaining + "\n", generalColor.ToNativeColor(), size);
						}
						else
						{
							builder.Append(withNewLine, blockHeaderColor.ToNativeColor(), size);
						}
					}
					else
					{
						builder.Append(withNewLine, generalColor.ToNativeColor(), size);
					}
				}
			}
#if __ANDROID__
			TextFormatted = builder.Build();
#elif __IOS__
			AttributedText = builder.Build();
			this.SetNeedsDisplay();
#endif
			System.Diagnostics.Debug.WriteLine("Text highlighting took: " + watch.ElapsedMilliseconds + " milliseconds");
			watch.Stop();
		}

#if __ANDROID__
		public int ContentHeight
		{
			get
			{
				int padding = CompoundPaddingTop + CompoundPaddingBottom;

				if ((int)Android.OS.Build.VERSION.SdkInt >= 16)
				{

					return (int)Math.Round((LineCount * (LineHeight + LineSpacingExtra) * LineSpacingMultiplier)) + padding;
				}

				return LineCount * LineHeight + padding;
			}
		}
#elif __IOS_
#endif
	}
}
