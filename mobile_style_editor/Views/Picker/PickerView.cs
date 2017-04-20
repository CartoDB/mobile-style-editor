using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class PickerView : BaseView
	{
		public PickerViewItem Drive { get; private set; }

		public FileListPopup Popup { get; private set; }

		public PickerView()
		{
			Drive = new PickerViewItem("icon_drive.png", "CHOOSE FROM GOOGLE DRIVE");

			Popup = new FileListPopup();
		}

		public override void LayoutSubviews()
		{
			Popup.Hide();
			double h = Height / 2;

			double max = 250;
			if (h > max)
			{
				h = max;
			}

			double w = h;
			double y = Height / 2 - h / 2;
			double x = Width / 2 - w / 2;

			AddSubview(Drive, x, y, w, h);

			AddSubview(Popup, 0, 0, Width, Height);
		}

	}
}
