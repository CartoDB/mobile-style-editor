
using System;
using System.Linq.Expressions;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class BaseView : RelativeLayout
	{
		Constraint ZeroConstraint
		{ 
			get { return GetConstraint(0); } 
		}
		public BaseView()
		{
			SizeChanged += OnSizeChanged;
		}

		void OnSizeChanged(object sender, EventArgs e)
		{
			Children.Clear();

			LayoutSubviews();
		}

		public void AddSubview(View view, double x, double y, double w, double h)
		{
			Children.Add(view, GetConstraint(x), GetConstraint(y), GetConstraint(w), GetConstraint(h));
		}

		/*
		 * Hack for rezising and changing position of elements, 
		 * as it's not an operation supported by default
		 * 
		 * Be sure to call ForceLayout() on the view's parent after rezising element
		 */
		public void UpdateLayout(double x, double y, double w, double h)
		{
			var constraint = BoundsConstraint.FromExpression(() => new Rectangle(x, y, w, h), new View[0]);
			SetBoundsConstraint(this, constraint);
		}

		Constraint GetConstraint(double number)
		{
			return Constraint.Constant(number);
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
