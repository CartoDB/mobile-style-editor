using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class PickerView : BaseView
	{
		Label titleLabel;

		public PickerViewItem Drive { get; private set; }
		public PickerViewItem Database { get; private set; }

		public FileListPopup Popup { get; private set; }

		public PickerView()
		{
			titleLabel = new Label();
			titleLabel.TextColor = Colors.CartoNavy;
			titleLabel.FontSize = 20f;

			titleLabel.VerticalTextAlignment = TextAlignment.Center;
			titleLabel.HorizontalTextAlignment = TextAlignment.Center;
			titleLabel.Text = "CHOOSE STYLE SOURCE";

            string folder = "";

#if __UWP__
            folder = "Assets/";
#endif

            Drive = new PickerViewItem(folder +"icon_drive.png", "FROM GOOGLE DRIVE");
			Database = new PickerViewItem(folder + "icon_database.png", "FROM LOCAL DATABASE");

			Popup = new FileListPopup();
		}

		public override void LayoutSubviews()
		{
			Popup.Hide();

			double x = 0;
			double y = 0;
			double h = Height / 8;
			double w = Width;

			AddSubview(titleLabel, x, y, w, h);

			double itemSize = 200;
			double itemPadding = 10;

			h = itemSize;
			w = itemSize;
			y = Height / 2 - h / 2;
			x = Width / 2 - w / 2;

			// PickerViewItem count
			int count = 2;

			x = Width / 2 - (count * itemSize + (count + 1) * itemPadding) / 2;

			AddSubview(Drive, x, y, w, h);

			x += itemSize + itemPadding;

			AddSubview(Database, x, y, w, h);

			AddSubview(Popup, 0, 0, Width, Height);
		}

	}
}
