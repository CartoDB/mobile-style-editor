
using System;
using mobile_style_editor;
using Xamarin.Forms;

#if __IOS__
using Xamarin.Forms.Platform.iOS;
#elif __ANDROID__
using Xamarin.Forms.Platform.Android;
#elif __UWP__
using Xamarin.Forms.Platform.UWP;
#endif

[assembly: ExportRenderer(typeof(MeasuredLabel), typeof(MeasuredLabelRenderer))]
namespace mobile_style_editor
{
    public class MeasuredLabelRenderer : LabelRenderer
    {
        MeasuredLabel View { get; set; }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                View = (MeasuredLabel)e.NewElement;

#if __ANDROID__

#elif __IOS__
                Control.Lines = View.NumberOfLines;
#endif
				Measure();

                View.TextUpdate += (sender, args) =>
                {
                    Measure();
                };
            }
        }

        public void Measure()
        {
#if __IOS__
            SizeToFit();
            double width = Frame.Width;
            double height = Frame.Height;
#elif __ANDROID__
			
            Control.Measure(0, 0);

            double density = Forms.Context.Resources.DisplayMetrics.Density;
            double width = Control.MeasuredWidth / density;
            double height = Control.MeasuredHeight / density;
#endif
            View.MeasuredWidth = width;
            View.MeasuredHeight = height;

            View.Measured?.Invoke(this, EventArgs.Empty);
        }

    }
}
