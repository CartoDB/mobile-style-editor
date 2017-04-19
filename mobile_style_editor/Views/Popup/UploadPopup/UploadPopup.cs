using System;
namespace mobile_style_editor
{
	public class UploadPopup : BasePopup
	{
		public new UploadPopupContent Content { get { return (UploadPopupContent)base.Content; } }

		public UploadPopup()
		{
			base.Content = new UploadPopupContent();
		}

		public override void LayoutSubviews()
		{
			double verticalPadding, horizontalPadding;

			if (Width > Height)
			{
				horizontalPadding = Width / 6;
				verticalPadding = Height / 10;
			}
			else
			{
				horizontalPadding = Width / 12;
				verticalPadding = Height / 4;
			}

			double x = horizontalPadding;
			double y = 20;
			double h = Height - 2 * verticalPadding;
			double w = Width - 2 * horizontalPadding;

			AddSubview(Content, x, y, w, h);
		}
	}
}
