using System;
namespace mobile_style_editor
{
	public class ConfirmationPopup : BasePopup
	{
		public new ConfirmationPopupContent Content { get { return (ConfirmationPopupContent)base.Content; } }

		public ConfirmationPopup()
		{
			base.Content = new ConfirmationPopupContent();
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
