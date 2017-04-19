
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
#if __ANDROID__
	public class NativeView : Android.Widget.RelativeLayout
	{
		public NativeView() : base(Forms.Context)
		{
#elif __IOS__
	public class NativeView : UIKit.UIView
	{
		public NativeView()
		{
#elif __UWP__
    public class NativeView : Windows.UI.Xaml.Controls.Control
    {
        public NativeView()
        {
#endif
        }

		Color backgroundColor;
		public
#if __IOS__
		new
#endif
		Color BackgroundColor
		{
			get { return backgroundColor; }
			set
			{
				backgroundColor = value;
#if __IOS__
				base.BackgroundColor = backgroundColor.ToNativeColor();
#elif __ANDROID__
				SetBackgroundColor(backgroundColor.ToNativeColor());
#endif
			}
		}

#if __IOS__
		public double Width { get { return (double)Frame.Width; } }
		public double Height { get { return (double)Frame.Height; } }
#elif __ANDROID__
#endif

#if __ANDROID__
		public void AddSubview(Android.Views.View view)
		{
			
		}
#elif __IOS__
		public void AddSubview(UIKit.UIView view, double x, double y, double w, double h)
		{
			view.Frame = new CoreGraphics.CGRect(x, y, w, h);
			AddSubview(view);
		}
#endif
		                       
		public virtual void OnTap(int x, int y)
		{

		}

#if __ANDROID__
		public override bool OnTouchEvent(Android.Views.MotionEvent e)
		{
			int x = (int)e.GetX();
			int y = (int)e.GetY();

			if (e.Action == Android.Views.MotionEventActions.Up)
			{
				OnTap(x, y);
				return true;
			}

			return false;
		}
#elif __IOS__
		public override void TouchesEnded(Foundation.NSSet touches, UIKit.UIEvent evt)
		{
			base.TouchesEnded(touches, evt);

			CoreGraphics.CGPoint point = (touches.AnyObject as UIKit.UITouch).LocationInView(this);

			int x = (int)point.X;
			int y = (int)point.Y;

			OnTap(x, y);
		}
#endif

	}
}
