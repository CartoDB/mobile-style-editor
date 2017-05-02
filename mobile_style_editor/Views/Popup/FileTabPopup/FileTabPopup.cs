using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class FileTabPopup : BaseView
	{
		public new bool IsVisible { get { return TranslationY > (visibleY - 0.1); } }

		public FileTabPopup()
		{
			BackgroundColor = Colors.CartoNavy;
		}

		double visibleY;

		public void Initialize(MainView parent, ZipData data)
		{
			double rowCount = Math.Ceiling((double)data.DecompressedFiles.Count / 3);
			double rowHeight = parent.Toolbar.Height;

			double h = rowCount * rowHeight;

			double w = parent.Width / 3;
			double x = 0;
			double y = 0;

			visibleY = rowHeight;

			parent.AddSubview(this, x, y, w, h);

			this.TranslateTo(0, -Height, 0);
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
			this.TranslateTo(0, visibleY);
		}

		public void Hide()
		{
			this.TranslateTo(0, -Height);
		}

	}
}
