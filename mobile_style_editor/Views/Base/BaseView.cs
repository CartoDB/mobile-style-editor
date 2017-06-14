
using System;
using System.Linq.Expressions;
using Xamarin.Forms;

namespace mobile_style_editor
{
    public class BaseView : RelativeLayout
    {
        public EventHandler<EventArgs> CornerRadiusSet;
        int cornerRadius;
        public int CornerRadius
        {
            get { return cornerRadius; }
            set
            {
                cornerRadius = value;
                if (CornerRadiusSet != null)
                {
                    CornerRadiusSet(cornerRadius, EventArgs.Empty);
                }
            }
        }

        const string ConstraintProperty_Height = "height";

        public static readonly double MatchParent = -1;

        public bool ClearChildrenOnLayout { get; set; } = true;

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
            LayoutSubviews();
        }

        public virtual void AddSubview(View view)
        {
            Children.Add(view, ZeroConstraint, ZeroConstraint, ZeroConstraint, ZeroConstraint);
        }

        public virtual void AddSubview(View view, double x, double y, double w, double h)
        {
            if (view.Parent == null)
            {
                Children.Add(view, GetConstraint(x), GetConstraint(y), GetConstraint(w), GetConstraint(h, ConstraintProperty_Height));
            }
            else
            {
                var constraint = BoundsConstraint.FromExpression(() => new Rectangle(x, y, w, h), new View[0]);
                SetBoundsConstraint(view, constraint);

                if (view.Parent is RelativeLayout)
                {
                    (view.Parent as RelativeLayout).ForceLayout();
                }
            }
        }

        public void UpdateLayout(double y, double height)
        {
            UpdateLayout(X, y, Width, height);
        }

        public virtual void RemoveChild(View view)
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

            if (Parent != null && Parent is RelativeLayout)
            {
                (Parent as RelativeLayout).ForceLayout();
            }
        }

        Constraint GetConstraint(double number, string property = "")
        {
            if (number < 0 && number > -2)
            {
                if (property.Equals(ConstraintProperty_Height))
                {
                    return Constraint.RelativeToParent((parent) => { return parent.Height; });
                }
                else
                {
                    return Constraint.RelativeToParent((parent) => { return parent.Width; });
                }
            }

            return Constraint.Constant(number);
        }

        public virtual void LayoutSubviews()
        {
            if (Loader != null && Loader.IsRunning)
            {
                RemoveChild(Loader);
                Loader = null;
                ShowLoading();
            }
        }

        public CustomLoader Loader { get; private set; }

        public void ShowLoading()
        {
            if (Loader == null)
            {
                Loader = new CustomLoader();

#if __IOS__
                Loader.BackgroundColor = Colors.LightTransparentGray;
                Loader.CornerRadius = 10;

#endif
                double size = 50;
				AddSubview(Loader, Width / 2 - size / 2, Height / 2 - size / 2, size, size);
                RaiseChild(Loader);
			}

			Loader.IsRunning = true;
		}

		public void HideLoading()
		{
			if (Loader != null)
			{
				Loader.IsRunning = false;
			}
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

#region Device Info

		public bool IsSmallScreen { get { return !IsTablet && !IsLandscape; } }

		public double Ratio
		{
			get
			{
				double width = EditorApplication.Width;
				double height = EditorApplication.Height;

				return height > width ? height / width : width / height;
			}
		}

		public bool IsTablet
		{
			get
			{
#if __IOS__
				return UIKit.UIDevice.CurrentDevice.Model.Contains("iPad");
#else
				/*
				 * TODO 
				 * There is no functional difference between an Android tablet and a phone
				 * Determine it based on height/width ratio
				 * 
				 * Pulled these constants out of my ass. May need to tweak.
				 * 
				 * Alternative would be:
				 * http://stackoverflow.com/questions/11330363/how-to-detect-device-is-android-phone-or-android-tablet
				 * 
				 */

				if (!IsLandscape)
				{
					// 1.22 on Nutiteq's Samsung Tablet
					// 1.42 on Aare's sony Z1
					return Ratio < 1.4;
				}

				// 1.50 on Nutiteq's Samsung Tablet
				// 2.08 on Aare's Sony Z1
				return Ratio < 1.7;
#endif
			}
		}

		public bool IsLandscape { get { return Height < Width; } }
	}
#endregion
}