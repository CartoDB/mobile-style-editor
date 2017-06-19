
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
    public class MeasuredLabel : Label
    {
        public EventHandler<EventArgs> Click;

        public bool IsClickable { get { return Click != null; } }

        public double MeasuredHeight { get; set; }

        public double MeasuredWidth { get; set; }

        public int NumberOfLines { get; set; }

        public EventHandler<EventArgs> TextUpdate;

        public EventHandler<EventArgs> Measured;

        public new string Text
        {
            get { return base.Text; }
            set 
            {
                base.Text = value;

                Measure();
            }
        }

        public void Measure()
        {
            TextUpdate?.Invoke(this, EventArgs.Empty);
        }
    }
}
