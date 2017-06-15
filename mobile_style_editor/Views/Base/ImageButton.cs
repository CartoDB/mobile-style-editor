
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
    public class ImageButton : ClickView
    {
		Image image;

        double padding;
        public double ImagePadding
        {
            get { return padding; } 
            set { padding = value; }
        }

		double topPadding = -1;
		public double TopPadding
		{
			get
			{
				if (topPadding > 0)
				{
					return topPadding;
				}

				return ImagePadding;
			}
			set { leftPadding = value; }
		}

        double leftPadding = - 1;
        public double LeftPadding
        {
            get
            {
                if (leftPadding > 0)
                {
                    return leftPadding;
                }

                return ImagePadding;
            }
            set { leftPadding = value; }
        }

        public string Source 
        {
            get { return image.Source.StyleId; } 
            set { image.Source = ImageSource.FromFile(value); }
        }

        public ImageButton()
        {
            image = new Image();
        }

        public override void LayoutSubviews()
        {
            double x = LeftPadding;
            double y = ImagePadding;
            double h = Height - 2 * padding;
            double w = Width - 2 * padding;

            AddSubview(image, x, y, w, h);
        }
    }
}
