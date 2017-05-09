using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class AddStyleItem : BaseView
	{
		BaseView container;

		Label titleLabel;

		public PickerViewItem Github { get; private set; }
		public PickerViewItem Drive { get; private set; }

		public AddStyleItem()
		{
			BackgroundColor = Color.White;

			container = new BaseView();
			container.BackgroundColor = Colors.NearWhite;

			titleLabel = new Label();
			titleLabel.TextColor = Colors.CartoNavy;
			titleLabel.FontSize = 13;
			titleLabel.FontAttributes = FontAttributes.Bold;

			titleLabel.VerticalTextAlignment = TextAlignment.Center;
			titleLabel.Text = "ADD YOUR OWN STYLE";

            string folder = "";

#if __UWP__
            folder = "Assets/";
#endif
			Github = new PickerViewItem(folder + "icon_github.png", "GITHUB");
			Github.TextSize = 10;
			Github.BackgroundColor = container.BackgroundColor;

			Drive = new PickerViewItem(folder + "icon_drive.png", "GOOGLE DRIVE");
			Drive.TextSize = 10;
			Drive.BackgroundColor = container.BackgroundColor;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			double padding = 5;

			AddSubview(container, padding, padding, Width - 2 * padding, Height - 2 * padding);

			double titleHeight = container.Height / 5;

			double x = padding;
			double y = 0;
			double w = Width;
			double h = titleHeight;

			container.AddSubview(titleLabel, x, y, w, h);

			double itemSize = container.Height - (titleHeight + 2 * padding);
			double itemPadding = 10;

			h = itemSize;
			w = itemSize;
			y = container.Height / 2 - h / 2;
			x = container.Width / 2 - w / 2;

			// PickerViewItem count
			int count = 3;

			x = container.Width / 2 - (count * itemSize + (count + 1) * itemPadding) / 2;
			x = container.Width - (2 * itemSize + 2 * itemPadding);

			container.AddSubview(Github, x, y, w, h);

			x += itemSize + itemPadding;

			container.AddSubview(Drive, x, y, w, h);
		}
	}
}
