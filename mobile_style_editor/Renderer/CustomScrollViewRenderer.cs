
using System;
using mobile_style_editor;
using Xamarin.Forms;

#if __IOS__
using Xamarin.Forms.Platform.iOS;
#elif __ANDROID__
using Android.Views;
using Xamarin.Forms.Platform.Android;
#elif __UWP__
using Xamarin.Forms.Platform.UWP;
#endif

[assembly: ExportRenderer(typeof(BaseScrollView), typeof(CustomScrollViewRenderer))]
namespace mobile_style_editor
{
    public class CustomScrollViewRenderer : ScrollViewRenderer
    {
        BaseScrollView View { get; set; }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                View = (BaseScrollView)e.NewElement;
#if __IOS__
                ScrollEnabled = View.ScrollEnabled;
#endif
            }
        }

#if __ANDROID__
        static readonly VelocityTracker tracker = VelocityTracker.Obtain();
        System.Timers.Timer timer;

        public override bool OnTouchEvent(MotionEvent ev)
        {
            if (ev.Action == MotionEventActions.Move)
            {
                tracker.AddMovement(ev);
            }
            if (ev.Action == MotionEventActions.Up)
            {
                tracker.ComputeCurrentVelocity(1000);
                var velocity = tracker.XVelocity;
                Console.WriteLine("Velocity: " + velocity);
                tracker.Clear();

                if (Math.Abs(velocity) > 1000)
                {
                    timer = new System.Timers.Timer(125);
                    timer.Start();
                    timer.Elapsed += delegate
                    {
                        CallHandler();
                        timer.Stop();
                        timer.Dispose();
                        timer = null;
                    };
                }
                else
                {
                    CallHandler();
                }
            }

            return base.OnTouchEvent(ev);
        }

        void CallHandler()
        {
            if (View.DecelerationEnded != null)
            {
                View.DecelerationEnded(null, EventArgs.Empty);
            }
        }
#endif
    }
}
