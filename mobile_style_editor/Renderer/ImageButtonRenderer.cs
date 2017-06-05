
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

[assembly: ExportRenderer(typeof(ImageButton), typeof(ImageButtonRenderer))]
namespace mobile_style_editor
{
    public class ImageButtonRenderer : VisualElementRenderer<RelativeLayout>
    {
        ImageButton View { get; set; }

        protected override void OnElementChanged(ElementChangedEventArgs<RelativeLayout> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                View = e.NewElement as ImageButton;

                if (View.CornerRadius!= 0)
                {
                    SetCornerRadius(View.CornerRadius);
                }

                View.CornerRadiusSet += OnCornerRadius;
            }
        }

        void OnCornerRadius(object sender, EventArgs e)
        {
            SetCornerRadius((int)sender);
        }

        void SetCornerRadius(int radius)
        {
#if __IOS__
            Layer.CornerRadius = radius;
#elif __ANDROID__
			var existing = View.BackgroundColor.ToNativeColor();

			var drawable = new Android.Graphics.Drawables.GradientDrawable();
			drawable.SetCornerRadius(View.CornerRadius);
			drawable.SetColor(existing.ToArgb());

            Background = drawable;
#endif
		}
    }
}
