
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class BaseView : RelativeLayout
	{
		public BaseView()
		{
			SizeChanged += OnSizeChanged;
		}

		void OnSizeChanged(object sender, EventArgs e)
		{
			Children.Clear();

			LayoutSubviews();
		}

		public void AddSubview(View view, Rectangle bounds)
		{
			Children.Add(view,
						 Constraint.Constant(bounds.X),
						 Constraint.Constant(bounds.Y),
						 Constraint.Constant(bounds.Width),
						 Constraint.Constant(bounds.Height)
						);
		}

		public void AddSubview(View view, double x, double y, double w, double h)
		{
			Children.Add(view,
						 Constraint.Constant(x),
						 Constraint.Constant(y),
						 Constraint.Constant(w),
						 Constraint.Constant(h)
						);
		}

		public virtual void LayoutSubviews() { Children.Clear(); }


		public ActivityIndicator Loader { get; private set; }

		public void ShowLoading()
		{
			if (Loader == null)
			{
				Loader = new ActivityIndicator();
				double size = 50;
				AddSubview(Loader, Width / 2 - size / 2, Height / 2 - size / 2, size, size);
			}

			Loader.IsRunning = true;
		}

		public void HideLoading()
		{
			Loader.IsRunning = false;
		}

	}
}
