using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class FileTabPopup : BaseView
	{
		public new bool IsVisible { get { return Math.Abs(TranslationY) < 0.1; } }

		public FileTabPopup()
		{
			BackgroundColor = Colors.CartoNavy;
		}

		public void Initialize(MainView parent, ZipData data)
		{
			double w = parent.Width / 3;
			double h = 200;
			double x = 0;
			double y = -h;
			y = 0;
			parent.AddSubview(this, x, y, w, h);
		}

		public void Toggle()
		{
			if (IsVisible)
			{
				Hide();
			}
			else
			{
				Show();
			}
		}

		public void Show()
		{
			this.TranslateTo(0, 0);
		}

		public void Hide()
		{
			this.TranslateTo(0, -Height);
		}

	}
}
