using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace mobile_style_editor
{
	public class BaseScrollView : ScrollView
	{
		StackLayout content;

		public IList<View> Children { get { return content.Children; } }

		public BaseScrollView()
		{
			content = new StackLayout();
			Content = content;

			SizeChanged += OnSizeChanged;
		}

		void OnSizeChanged(object sender, EventArgs e)
		{
			LayoutSubviews();
		}

		public void AddSubview(View view, double h)
		{
			view.HeightRequest = h;
			content.Children.Add(view);
		}

		public void RemoveChild(View view)
		{
			content.Children.Remove(view);
		}

		public virtual void LayoutSubviews() { /*content.Children.Clear();*/ }
	}
}
