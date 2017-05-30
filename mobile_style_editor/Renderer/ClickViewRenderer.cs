
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

#if __UWP__

#else

[assembly: ExportRenderer(typeof(ClickView), typeof(ClickViewRenderer))]
namespace mobile_style_editor
{
	public class ClickViewRenderer : ViewRenderer
	{
		ClickView View { get; set; }

		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
			{
#if __IOS__
				SetNativeControl(new UIKit.UIView());
#elif __ANDROID__
#elif __UWP__
#endif
			}

			if (e.NewElement != null)
			{
				View = e.NewElement as ClickView;
#if __ANDROID__
				SetBackgroundColor(View.BackgroundColor.ToNativeColor());

				Touch += (object sender, TouchEventArgs args) =>
				{
					if (!View.IsEnabled)
					{
						return;
					}

					var action = args.Event.Action;

					if (action == Android.Views.MotionEventActions.Down)
					{
						Alpha = 0.5f;
					}
					else if (action == Android.Views.MotionEventActions.Up)
					{
						Alpha = 1.0f;
						View.Click?.Invoke(View, EventArgs.Empty);

					}
					else if (action == Android.Views.MotionEventActions.Cancel)
					{
						Alpha = 1.0f;
					}
				};
#else
				Control.BackgroundColor = View.BackgroundColor.ToNativeColor();
#endif
			}
		}
#if __IOS__
		public override void TouchesBegan(Foundation.NSSet touches, UIKit.UIEvent evt)
		{
			base.TouchesBegan(touches, evt);
			View.Opacity = 0.5f;
		}

		public override void TouchesEnded(Foundation.NSSet touches, UIKit.UIEvent evt)
		{
			base.TouchesEnded(touches, evt);
			View.Opacity = 1.0f;
		}

		public override void TouchesCancelled(Foundation.NSSet touches, UIKit.UIEvent evt)
		{
			base.TouchesCancelled(touches, evt);
			View.Opacity = 1.0f;
		}
#elif __UWP__

#endif
	}
}
#endif