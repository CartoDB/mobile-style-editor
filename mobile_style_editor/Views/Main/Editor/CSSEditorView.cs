﻿
using System;
using Xamarin.Forms;

#if __IOS__
using Xamarin.Forms.Platform.iOS;
#elif __ANDROID__
using Xamarin.Forms.Platform.Android;
#elif __UWP__
using Xamarin.Forms.Platform.UWP;
#endif

namespace mobile_style_editor
{
    public class CSSEditorView : BaseView
    {
#if __IOS__
        public static UIKit.UIFont Font = UIKit.UIFont.FromName("Menlo-Regular", 11);
#endif
        ZipData items;

#if __IOS__
        UIKit.UITextView lineNumbers;
#endif
        public EditorField Field { get; private set; }
        public WarningPopup Popup { get; private set; }

        public string Text { get { return Field.Text; } }

        public CSSEditorView()
        {
            Field = new EditorField();

            Popup = new WarningPopup();

#if __IOS__
            lineNumbers = new UIKit.UITextView();
            lineNumbers.BackgroundColor = Field.BackgroundColor.ToNativeColor();

		    Field.OffsetChanged += delegate {
		        lineNumbers.ContentOffset = Field.ContentOffset;
		    };
#endif
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            double x = 0;
            double y = 0;
            double w = Width;
            double h = Height;

#if __IOS__
            w = 25;
            AddSubview(lineNumbers.ToView(), x, y, w, h);
            x += w;
            w = Width - w;
#endif

#if __ANDROID__
			Field.RemoveFromParent();
#elif __UWP__
            if (Field.Parent != null)
            {
                (Field.Parent as NativeViewWrapperRenderer).Children.Remove(Field);
            }
#endif
            AddSubview(Field.ToView(), x, y, w, h);

            AddSubview(Popup, x, y, w, h);
            RaiseChild(Popup);
        }

        public void Initialize(ZipData items)
        {
            this.items = items;
            Update(0);
        }

        public void Update(int index)
        {
            string text = items.DecompressedFiles[index];
			Field.currentSelection = -1;
            Field.currentText = null;
            Field.Update(text);
#if __IOS__
            string[] lines = text.Split('\n');
            nfloat padding = 2;
            nfloat fieldInset = 8;

            nfloat x = padding;
            nfloat y = fieldInset;
            nfloat w = lineNumbers.Frame.Width - 2 * padding;
            nfloat h = Font.LineHeight;

            foreach (var subview in lineNumbers.Subviews)
            {
                if (subview is UIKit.UILabel)
                {
                    subview.RemoveFromSuperview();
                }
            }

            var color = UIKit.UIColor.FromRGBA(150, 150, 150, 150);

            // HACK Increase line count to account for wrapping
            lines = new string[lines.Length + 50];        

            for (int i = 1; i < lines.Length + 1; i++)
            {
                var label = new UIKit.UILabel();
                label.TextColor = color;
                label.TextAlignment = UIKit.UITextAlignment.Right;
                label.Font = Font;
                
                label.Text = i.ToString();

                lineNumbers.AddSubview(label);

                label.Frame = new CoreGraphics.CGRect(x, y, w, h);
                y += h;
				// TODO WordWrapping -> Currently multi-line lines are counted as two sepate lines
				//    var line = lines[i];
				//    var nsstring = new Foundation.NSString(line);

				//    var max = Field.Frame.Width / (h / 1.4);
				//    if (line.Length > max)
				//    {
				//        y += h;
				//        Console.WriteLine("Line 1: " + line + " " + line.Length + ", max: " + max + ")");
				//    }

				//    if (line.Length > max * 2)
				//    {
				//        y += h;
				//Console.WriteLine("Line 2: " + line + " " + line.Length + ", max: " + max + ")");
				//}
			}
#endif
        }

    }
}
