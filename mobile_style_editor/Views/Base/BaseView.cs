
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

		public void RemoveChild(View view)
		{
			Children.Remove(view);
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

        Label toast;
        System.Threading.Timer timer;
        public void Toast(string text)
        {
            if (toast == null)
            {
                toast = new Label();
                toast.VerticalTextAlignment = TextAlignment.Center;
                toast.HorizontalTextAlignment = TextAlignment.Center;
                toast.BackgroundColor = Color.FromRgba(0, 0, 0, 190);
                toast.TextColor = Color.White;

                double padding = 30;
                double w = 300;
                double h = 35;
                double x = Width / 2 - w / 2;
                double y = Height - (h + padding);

                AddSubview(toast, x, y, w, h);
            }
            toast.Text = text;
            toast.FadeTo(1.0);
            
            if (timer != null)
            {
                timer.Dispose();
                timer = null;
            }

            timer = new System.Threading.Timer((object state) =>
            {
                Device.BeginInvokeOnMainThread(delegate
                {
                    toast.FadeTo(0.0);
                    timer = null;
                });
            }, null, 500, System.Threading.Timeout.Infinite);
        }

    }
}
