using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace mobile_style_editor
{
	public class BaseScrollView : ScrollView
	{
		BaseView content;

		public IList<View> Children { get { return content.Children; } }

		Constraint ZeroConstraint
		{
			get { return GetConstraint(0); }
		}

		public BaseScrollView()
		{
			content = new BaseView();
			content.ClearChildrenOnLayout = false;
			Content = content;

			SizeChanged += OnSizeChanged;
		}

		void OnSizeChanged(object sender, EventArgs e)
		{
			LayoutSubviews();
		}

		public void AddSubview(View view)
		{
			content.AddSubview(view);
			//content.Children.Add(view, ZeroConstraint, ZeroConstraint, ZeroConstraint, ZeroConstraint);
		}

		public void AddSubview(View view, double x, double y, double w, double h)
		{
			content.AddSubview(view, x, y, w, h);
			//content.Children.Add(view, GetConstraint(x), GetConstraint(y), GetConstraint(w), GetConstraint(h));
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
