using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class BaseScrollView : ScrollView
	{
		StackLayout content;

		public BaseScrollView()
		{
			content = new StackLayout();
			Content = content;
			SizeChanged += OnSizeChanged;

			Padding = new Thickness(0, 0, 0, 0);
			content.Padding = new Thickness(0, 0, 0, 0);
			content.Margin = new Thickness(0, 0, 0, 0);
		}

		void OnSizeChanged(object sender, EventArgs e)
		{
			content.Children.Clear();

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

		public virtual void LayoutSubviews() { content.Children.Clear(); }
	}
}
