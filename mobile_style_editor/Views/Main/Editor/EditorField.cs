
using System;
using Xamarin.Forms;
using System.Collections.Generic;

#if __IOS__
using Xamarin.Forms.Platform.iOS;
using UIKit;

#elif __ANDROID__
using Xamarin.Forms.Platform.Android;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Android.Text;

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
        System.Timers.Timer updateTimer;

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
        
		public void InitializeTimer()
		{
            updateTimer = new System.Timers.Timer(1000);
			updateTimer.Elapsed += UpdateText;
			updateTimer.Start();
		}

		public void DisposeTimer()
		{
            updateTimer.Stop();
            updateTimer.Dispose();
            updateTimer = null;
		}

		// Currently only used in iOS, 
		// as Droid does not have a solid API for recognizing scroll, nor is it necessary
		public bool IsScrolling { get; private set; }

        public void UpdateText(object sender, EventArgs e)
        {
            if (currentText == null)
            {
                Console.WriteLine("Won't update Text: currentText == null");
                return;
            }

			if (currentSelection == -1)
			{
				Console.WriteLine("Won't update Text: currentSelection == -1");
                return;
			}

			if (IsScrolling)
			{
				Console.WriteLine("Won't update Text: IsScrolling");
				return;
			}

            Console.WriteLine("Updating Editor Text");
            Update(currentText, currentSelection);
        }

        const string NewLine = "\n";
        const string Backspace = "";

        int currentSelection = -1;
        string currentText;

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

        public override IInputConnection OnCreateInputConnection(EditorInfo outAttrs)
        {
            return new KeyStrokeListener(this, base.OnCreateInputConnection(outAttrs));
        }

		public override void OnEditorAction(ImeAction actionCode)
		{
			if (actionCode == ImeAction.Done)
			{
				if (SelectionStart == TextFormatted.Length())
				{
					return;
				}

                currentText = currentText.Insert(SelectionStart, NewLine);
				Update(currentText, SelectionStart + NewLine.Length);
			}
		}

        protected class KeyStrokeListener : InputConnectionWrapper
        {
            EditorField view;

            public KeyStrokeListener(EditorField view, IInputConnection target) : base(target, true)
            {
                this.view = view;
            }

            public override bool SendKeyEvent(KeyEvent e)
            {
                var action = e.Action;
                var code = e.KeyCode;

                if (action == KeyEventActions.Up)
                {
                    if (code == Keycode.Del)
                    {
                        view.RemoveNewLine();
                    }
                }
                return base.SendKeyEvent(e);
            }
        }

        public void RemoveNewLine()
        {
            int selection = SelectionStart;

            string substring = currentText.Substring(selection - 1, 1);

            if (substring.Equals(NewLine))
            {
                currentText = currentText.Remove(selection - 1, 1);
                Update(currentText, selection - 1);
            }
        }

        protected override void OnScrollChanged(int l, int t, int oldl, int oldt)
        {
            ClearFocus();

            CloseKeyboard();

            base.OnScrollChanged(l, t, oldl, oldt);
        }

        void CloseKeyboard()
        {
            var service = Forms.Context.GetSystemService(Android.Content.Context.InputMethodService);
            var manager = (InputMethodManager)service;
            manager.HideSoftInputFromWindow(WindowToken, 0);
        }

#elif __IOS__

        [Foundation.Export("textView:shouldChangeTextInRange:replacementText:")]
        public new bool ShouldChangeText(UITextView textView, Foundation.NSRange range, string text)
        {
            int selection = (int)range.Location;
            if (text.Equals(Backspace))
            {
                currentText = currentText.Remove(selection, 1);
            }
            else
            {
                currentText = currentText.Insert(selection, text);
            }
            return true;
        }

        bool initialized;

        [Foundation.Export("textViewDidChangeSelection:")]
        public new void SelectionChanged(UITextView textView)
        {
            if (!initialized)
            {
                // iOS calls this after text is initially entered,
                // with SelectedRange.Location == Text.Length;
                // Ignore the set so it wouldn't screw up selection 
                initialized = true;
                currentSelection = -1;
                return;
            }

            currentSelection = (int)textView.SelectedRange.Location;
        }

        [Foundation.Export("scrollViewWillBeginDragging:")]
        public new void DraggingStarted(UIScrollView scrollView)
        {
            if (IsFirstResponder)
            {
                ResignFirstResponder();
            }

            IsScrolling = true;
        }

        [Foundation.Export("scrollViewDidEndDragging:willDecelerate:")]
        public new void DraggingEnded(UIScrollView scrollView, bool willDecelerate)
        {
            if (!willDecelerate)
            {
                IsScrolling = false;
            }
        }

        [Foundation.Export("scrollViewDidEndDecelerating:")]
        public new void DecelerationEnded(UIScrollView scrollView)
        {
            IsScrolling = false;
        }

        public EventHandler<EventArgs> OffsetChanged;

        [Foundation.Export("scrollViewDidScroll:")]
        public new void Scrolled(UIScrollView scrollView)
        {
            if (OffsetChanged != null)
            {
                OffsetChanged(null, EventArgs.Empty);
            }
        }

#endif

        public void Update(string text, int selection = -1)
        {
            currentText = text;

            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            System.Threading.Tasks.Task.Run(delegate
            {
#if __UWP__
	            Device.BeginInvokeOnMainThread(delegate
	            {
	                Text = text;
	            });
#endif
				object result = CartoCSSParser.Parse(text);

                Device.BeginInvokeOnMainThread(delegate
                {
#if __ANDROID__
                    TextFormatted = (SpannableStringBuilder)result;

					if (selection != -1)
					{
						SetSelection(selection);
					}
#elif __IOS__

                    AttributedText = (Foundation.NSMutableAttributedString)result;

                    if (selection != -1)
                    {
                        SelectedRange = new Foundation.NSRange(selection, 0);
                        SetContentOffset(ContentOffset, true);
                    }
#elif __UWP__
                    Document.ApplyDisplayUpdates();
#endif
					System.Diagnostics.Debug.WriteLine("Text highlighting took: " + watch.ElapsedMilliseconds + " milliseconds");
					watch.Stop();
                });
            });
        }

    }
}
