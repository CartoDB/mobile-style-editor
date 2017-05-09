using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace mobile_style_editor
{
	public class BaseScrollView : ScrollView
	{
		RelativeLayout content;

		public IList<View> Children { get { return content.Children; } }

		public BaseScrollView()
		{
			content = new RelativeLayout();
			Content = content;

			SizeChanged += OnSizeChanged;
		}

		void OnSizeChanged(object sender, EventArgs e)
		{
			LayoutSubviews();
		}

		public void AddSubview(View view, double x, double y, double w, double h)
		{
			content.Children.Add(view, GetConstraint(x), GetConstraint(y), GetConstraint(w), GetConstraint(h));
		}
		Constraint GetConstraint(double number)
		{
			return Constraint.Constant(number);
		}

		public void RemoveChild(View view)
		{
			content.Children.Remove(view);
		}

		public virtual void LayoutSubviews() { /*content.Children.Clear();*/ }
	}
}
