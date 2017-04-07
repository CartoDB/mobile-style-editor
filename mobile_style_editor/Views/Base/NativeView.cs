
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

#endif
		}

		Color backgroundColor;
		public Color BackgroundColor
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

		public double Width { get { return (double)Frame.Width; } }
		public double Height { get { return (double)Frame.Height; } }

#if __ANDROID__
		public void AddSubview(UIKit.UIView view,
#elif __IOS__
		public void AddSubview(UIKit.UIView view,
#endif
							   double x, double y, double w, double h)
		{
			view.Frame = new CoreGraphics.CGRect(x, y, w, h);
			AddSubview(view);
		}

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
