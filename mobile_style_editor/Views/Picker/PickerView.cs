using System;
namespace mobile_style_editor
{
	public class PickerView : BaseView
	{
		public PickerViewItem Drive { get; private set; }

		public PickerView()
		{
			Drive = new PickerViewItem("icon_drive.png");
		}

		public override void LayoutSubviews()
		{
			double padding = 10;

			double h = Height / 2;

			double max = 300;
			if (h > max)
			{
				h = max;
			}

			double w = h;
			double x = 10;
			double y = Height / 2 - h / 2;

			AddSubview(Drive, x, y, w, h);
		}
	}
}
