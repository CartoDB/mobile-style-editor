using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace mobile_style_editor
{
    public static class ViewExtensions
    {
        public static void UpdateLayout(this View view, double x, double y, double w, double h)
        {
            var constraint = BoundsConstraint.FromExpression(() => new Rectangle(x, y, w, h), new View[0]);
            RelativeLayout.SetBoundsConstraint(view, constraint);
        }
    }
}
