
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class UploadPopupContent : BasePopupContent
	{
		Label nameLabel;
		Entry nameInput;

		public Button Confirm { get; private set; }

		public string Text { 
			get { return nameInput.Text; }
			set { nameInput.Text = value; }
		}

		public UploadPopupContent()
		{
			nameLabel = new Label();
			nameLabel.VerticalTextAlignment = TextAlignment.End;
			nameLabel.Text = "FILENAME";
			nameLabel.TextColor = Color.Gray;
			nameLabel.FontSize = 12f;

			nameInput = new Entry();
			nameInput.FontSize = 13f;
			nameInput.TextColor = Colors.CartoNavy;

			Confirm = new Button();
			Confirm.Text = "CONFIRM";
			Confirm.BackgroundColor = Colors.CartoRed;
			Confirm.TextColor = Color.White;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			double padding = 10;

			double w;

			if (Width > Height)
			{
				w = Width / 2;
			}
			else
			{
				w = Height / 2;
			}

			double h = 50;

			double x = Width / 2 - w / 2;
			double y = padding;

			AddSubview(nameLabel, x, y, w, h);

			y += h;

			AddSubview(nameInput, x, y, w, h);

			y += h;

			w = h * 3;
			x = Width / 2 - w / 2;

			AddSubview(Confirm, x, y, w, h);
		}

	}
}
