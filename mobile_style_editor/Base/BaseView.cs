
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class BaseView : AbsoluteLayout
	{
		public BaseView()
		{

			SizeChanged += OnSizeChanged;
		}

		void OnSizeChanged(object sender, EventArgs e)
		{
			LayoutSubviews();
		}

		public void AddSubview(View view)
		{
			Children.Add(view);
		}

		public virtual void LayoutSubviews() { }
	}
}
