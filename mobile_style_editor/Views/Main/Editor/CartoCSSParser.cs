
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
    public class CartoCSSParser
    {
		static float size = 1f;

		// White
		static readonly Color generalColor = Color.White;
		// Light gray
		static readonly Color commentColor = Color.FromRgb(120, 120, 120);
		// Teal
		static readonly Color constantColor = Color.FromRgb(0, 147, 146);
		// DarkGreen
		static readonly Color blockHeaderColor = Color.FromRgb(57, 177, 133);
		// LightGreen
		static readonly Color color3 = Color.FromRgb(156, 203, 134);
		// Wheat
		static readonly Color color4 = Color.FromRgb(233, 226, 156);
		// Pink Wheat
		static readonly Color classColor = Color.FromRgb(238, 180, 121);
		// Pink
		static readonly Color color6 = Color.FromRgb(232, 132, 113);
		// Dark Pink (magenta-ish)
		static readonly Color idColor = Color.FromRgb(207, 89, 126);

		public static
#if __ANDROID__
        Android.Text.SpannableStringBuilder
#elif __IOS__
        Foundation.NSMutableAttributedString
#elif __UWP__

#endif
        Parse(string text)
        {
			string[] lines = text.Split('\n');

#if __ANDROID__
			var builder = new Droid.SimpleSpanBuilder();
#elif __IOS__

            var builder = new iOS.AttributedTextBuilder();
#elif __UWP__
            var builder = new UWP.SimpleSpanBuilder(this);
#endif
			bool isInCommentBlock = false;

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
                    Color color = constantColor;

                    if (trimmed.Contains(":"))
                    {
                        string first = trimmed.Substring(0, 1);
                        string last = trimmed.Substring(trimmed.Length - 1, 1);

                        if (first.Equals(":") || last.Equals(":"))
                        {
                            color = generalColor;
                        }
                        else
                        {
                            color = constantColor;
                        }
                    }
                    else
                    {
                        color = generalColor;
                    }

                    builder.Append(withNewLine, color.ToNativeColor(), size);
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
                else if (trimmed.Contains("{"))
                {
                    Color color = blockHeaderColor;

                    if (trimmed.Contains("class"))
                    {
                        color = classColor;
                    }
                    else if (trimmed.Contains("#"))
                    {
                        color = idColor;    
                    }

                    int bracketIndex = line.IndexOf("{", StringComparison.Ordinal);
                    string blockHeader = line.Substring(0, bracketIndex);
                    string remaining = line.Substring(bracketIndex, line.Length - bracketIndex);

                    builder.Append(blockHeader, color.ToNativeColor(), size);
                    builder.Append(remaining + "\n", generalColor.ToNativeColor(), size);
                }
                else if (trimmed.Contains("["))
                {
                    if (trimmed.Contains("class"))
                    {
                        builder.Append(withNewLine, classColor.ToNativeColor(), size);
                    }
                    else
                    {
                        builder.Append(withNewLine, blockHeaderColor.ToNativeColor(), size);
                    }
                }
                else if (trimmed.Contains("#"))
                {
                    builder.Append(withNewLine, idColor.ToNativeColor(), size);
                }
                else
                {
                    builder.Append(withNewLine, generalColor.ToNativeColor(), size);
                }
            }

            return builder.Build();
        }
    }
}
