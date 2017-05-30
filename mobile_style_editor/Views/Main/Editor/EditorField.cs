﻿
using System;
using Xamarin.Forms;

#if __IOS__
using Xamarin.Forms.Platform.iOS;
using UIKit;
#elif __ANDROID__
using Xamarin.Forms.Platform.Android;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
#elif __UWP__
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
#endif

namespace mobile_style_editor
{
#if __IOS__
    public class EditorField : UITextView, IUITextViewDelegate
#elif __ANDROID__
	public class EditorField : EditText
#elif __UWP__
        public class EditorField : Windows.UI.Xaml.Controls.RichEditBox
#endif
    {
        public EventHandler<EventArgs> EditingEnded;

        Color textColor;
        public
#if __IOS__
        new
#endif
        Color TextColor
        {
            get { return textColor; }
            set
            {
                textColor = value;
#if __IOS__
                base.TextColor = backgroundColor.ToNativeColor();
#elif __ANDROID__
				SetTextColor(textColor.ToNativeColor());
#elif __UWP__
               
#endif
            }
        }

        Color backgroundColor;
        public
#if __IOS__
        new
#endif
        Color BackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                backgroundColor = value;
#if __IOS__
                base.BackgroundColor = backgroundColor.ToNativeColor();
#elif __ANDROID__
				SetBackgroundColor(backgroundColor.ToNativeColor());
#elif __UWP__
                // TODO This does nothing
                Background = new Windows.UI.Xaml.Media.SolidColorBrush(value.ToNativeColor());
#endif
            }
        }

#if __UWP__
        public string Text
        {
            get
            {
                string content = "";
                Document.GetText(Windows.UI.Text.TextGetOptions.None, out content);
                return content;
            }
            set
            {
                Document.SetText(Windows.UI.Text.TextSetOptions.None, value);
            }
        }
#endif
        public EditorField()
#if __ANDROID__
		: base(Forms.Context)
#endif
        {
#if __IOS__
            BackgroundColor = Color.FromRgb(20, 20, 20);
#else
			BackgroundColor = Color.FromRgb(50, 50, 50);
#endif
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

			ImeOptions = ImeAction.Done;
			SetRawInputType(Android.Text.InputTypes.ClassText);

#elif __IOS__
            ReturnKeyType = UIReturnKeyType.Done;
            Delegate = this;
            AutocorrectionType = UITextAutocorrectionType.No;

            RegisterForKeyboardNotifications();
#elif __UWP__
            Window.Current.CoreWindow.KeyDown += (s, e) =>
            {
                var ctrl = Window.Current.CoreWindow.GetKeyState(VirtualKey.Control);
                if (ctrl.HasFlag(CoreVirtualKeyStates.Down) && e.VirtualKey == VirtualKey.S)
                {
                    if (EditingEnded != null)
                    {
                        EditingEnded(this, EventArgs.Empty);
                    }
                }
            };
            
            BorderThickness = new Windows.UI.Xaml.Thickness(0, 0, 0, 0);

            Console.WriteLine(Resources);

            string hover = "PointerOver";
            string focused = "Focused";

            string textColor = "#ffffff";
            string backgroundColor = "#2a2a2a";

            Resources["TextControlForeground"] = textColor;
            Resources["TextControlForeground" + hover] = textColor;
            Resources["TextControlForeground" + focused] = textColor;

            Resources["TextControlBackground"] = backgroundColor;
            Resources["TextControlBackground" + hover] = backgroundColor;
            Resources["TextControlBackground" + focused] = backgroundColor;
            
            IsSpellCheckEnabled = false;
#endif
        }


        const string NewLine = "\n";
		const string Backspace = "\b";
        
		string current;

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

		public override void OnEditorAction(ImeAction actionCode)
		{
			if (actionCode == ImeAction.Done)
			{
				if (SelectionStart == TextFormatted.Length())
				{
					return;
				}

				current = current.Insert(SelectionStart, NewLine);
				Update(current, SelectionStart + NewLine.Length);

				//if (EditingEnded != null)
				//{
				//	var manager = (InputMethodManager)Context.GetSystemService("input_method");
				//	manager.ToggleSoftInput(ShowFlags.Forced, 0);

				//	EditingEnded(this, EventArgs.Empty);
				//}
			}
		}

#elif __IOS__

		[Foundation.Export("textView:shouldChangeTextInRange:replacementText:")]
        public new bool ShouldChangeText(UITextView textView, Foundation.NSRange range, string text)
        {
			int selection = (int)range.Location;

			if (text.Equals(NewLine))
			{
				current = current.Insert(selection, NewLine);
				Update(current, selection + 1);
			}
			else
			{
				// TODO check if text.Length == 0 -> if true && line is empty -> remove newline character
				//current = current.Insert(selection, text);
				//Update(current, selection);
			}

            return true;
        }
#endif

        public void Update(string text, int selection = -1)
        {
            current = text;

            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            System.Threading.Tasks.Task.Run(delegate
            {
                string[] lines = text.Split('\n');

#if __ANDROID__
				var builder = new Droid.SimpleSpanBuilder();
#elif __IOS__

                var builder = new iOS.AttributedTextBuilder();
#elif __UWP__
            var builder = new UWP.SimpleSpanBuilder(this);
#endif
                float size = 1f;

                // White
                Color generalColor = Color.White;
                // Light gray
                Color commentColor = Color.FromRgb(120, 120, 120);
                // Carto green
                Color blockHeaderColor = Color.FromRgb(145, 198, 112);
                // Dark magenta
                Color constantColor = Color.FromRgb(150, 0, 150);

                bool isInCommentBlock = false;

#if __UWP__
                Device.BeginInvokeOnMainThread(delegate
                {
                    Text = text;
                });
#endif

                foreach (string line in lines)
                {
                    string trimmed = line.Trim();
                    string withNewLine = line + "\n";

                    if (isInCommentBlock)
                    {
                        if (trimmed.Contains("*/"))
                        {
                            int nonCommentIndex = line.IndexOf("*/", StringComparison.Ordinal) + 2;
                            string comment = line.Substring(0, nonCommentIndex);
                            string nonComment = line.Substring(nonCommentIndex, line.Length - nonCommentIndex);

                            builder.Append(comment, commentColor.ToNativeColor(), size);
                            builder.Append(nonComment + "\n", generalColor.ToNativeColor(), size);
                            isInCommentBlock = false;
                        }
                        else
                        {
                            builder.Append(withNewLine, commentColor.ToNativeColor(), size);
                        }
                        continue;
                    }

                    if (trimmed.StartsWith("//", StringComparison.Ordinal))
                    {
                        builder.Append(withNewLine, commentColor.ToNativeColor(), size);
                    }
                    else if (trimmed.StartsWith("@", StringComparison.Ordinal))
                    {
                        builder.Append(withNewLine, constantColor.ToNativeColor(), size);
                    }
                    else if (trimmed.Contains("/*"))
                    {
                        int commentIndex = line.IndexOf("/*", StringComparison.Ordinal);
                        string nonComment = line.Substring(0, commentIndex);
                        string comment = line.Substring(commentIndex, line.Length - commentIndex);

                        builder.Append(nonComment, generalColor.ToNativeColor(), size);
                        builder.Append(comment + "\n", commentColor.ToNativeColor(), size);
                        isInCommentBlock = true;
                    }
                    else
                    {
                        if (trimmed.Contains("{"))
                        {
                            int bracketIndex = line.IndexOf("{", StringComparison.Ordinal);
                            string blockHeader = line.Substring(0, bracketIndex);
                            string remaining = line.Substring(bracketIndex, line.Length - bracketIndex);

                            builder.Append(blockHeader, blockHeaderColor.ToNativeColor(), size);
                            builder.Append(remaining + "\n", generalColor.ToNativeColor(), size);
                        }
                        else if (trimmed.Contains("#") || trimmed.Contains("["))
                        {
                            builder.Append(withNewLine, blockHeaderColor.ToNativeColor(), size);
                        }
                        else
                        {
                            builder.Append(withNewLine, generalColor.ToNativeColor(), size);
                        }
                    }
                }
                Device.BeginInvokeOnMainThread(delegate
                {
#if __ANDROID__
					TextFormatted = builder.Build();

					if (selection != -1)
					{
						SetSelection(selection);
					}
#elif __IOS__
                    AttributedText = builder.Build();
                    SetNeedsDisplay();
                    
                    if (selection != -1)
                    {
                        SelectedRange = new Foundation.NSRange(selection, 0);
                        SetContentOffset(ContentOffset, true);
                    }
#elif __UWP__
                    Document.ApplyDisplayUpdates();
#endif
                });

                System.Diagnostics.Debug.WriteLine("Text highlighting took: " + watch.ElapsedMilliseconds + " milliseconds");
                watch.Stop();
            });
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
#elif __IOS__

        void RegisterForKeyboardNotifications()
        {
			UIKeyboard.Notifications.ObserveWillShow(OnKeyboardShow);
			UIKeyboard.Notifications.ObserveWillHide(OnKeyboardHide);            
        }

		private void OnKeyboardShow(object sender, UIKeyboardEventArgs e)
		{
			// Need to calculate keyboard exact size due to Apple suggestions
			ScrollEnabled = true;
			float size = (float)e.FrameEnd.Height;

			var inset = new UIEdgeInsets(0, 0, size, 0);
			ContentInset = inset;
		}

		private void OnKeyboardHide(object sender, UIKeyboardEventArgs e)
        {
            ContentInset = new UIEdgeInsets(0, 0, 0, 0);
		}

#endif

    }

}
