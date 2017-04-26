using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;

namespace mobile_style_editor.UWP
{
    public class SimpleSpanBuilder
    {
        RichEditBox box;

        public SimpleSpanBuilder(RichEditBox field)
        {
            box = field;
        }

        string text = "";

        public void Append(string addition, Color color, float size)
        {
            int start = text.Length;
            int end = start + addition.Length;
            text += addition;

            ITextRange range = null;
            Xamarin.Forms.Device.BeginInvokeOnMainThread(delegate
            {
                range = box.Document.GetRange(start, end);
                range.CharacterFormat.ForegroundColor = color;
            });
        }

        public string Build()
        {
            return text;
        }
    }
}
