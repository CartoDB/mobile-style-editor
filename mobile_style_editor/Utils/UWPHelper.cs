using System;
using System.Collections.Generic;
using System.Text;

namespace mobile_style_editor
{
    public class UWPHelper
    {
    }
#if __UWP__
    public class Console
    {
        public static void WriteLine(object text)
        {
            System.Diagnostics.Debug.WriteLine(text);
        }
    }
#endif
}
