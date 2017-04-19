using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace mobile_style_editor.UWP
{
    public class SimpleSpanBuilder
    {
        string text = "";

        public void Append(string text, Color color, float size)
        {
            this.text += text;
        }

        public string Build()
        {
            return text;
        }
    }
}
