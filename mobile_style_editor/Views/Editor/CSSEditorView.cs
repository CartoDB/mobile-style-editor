
using System;
using Xamarin.Forms;

#if __IOS__
using Xamarin.Forms.Platform.iOS;
#elif __ANDROID__
using Xamarin.Forms.Platform.Android;
#elif __UWP__
using Xamarin.Forms.Platform.UWP;
#endif

namespace mobile_style_editor
{
	public class CSSEditorView : BaseView
	{
		ZipData items;

		public EditorField Field { get; private set; }

		public RefreshButton RefreshButton { get; private set; }

		public string Text { get { return Field.Text; } }

		public CSSEditorView()
		{
			Field = new EditorField();

			RefreshButton = new RefreshButton();
			RefreshButton.BackgroundColor = Colors.CartoRed;

            string folder = "";
#if __UWP__
            folder = "Assets/";
#endif

			RefreshButton.ImageSource = new FileImageSource { File = folder + "icon_refresh.png" };
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			double x = 0;
			double y = 0;
			double w = Width;
			double h = Height;

			AddSubview(Field.ToView(), x, y, w, h);

			double padding = 10;

			w = 50;
			h = w;
			x = Width - (w + padding);
			y = Height - (h + padding);

			//RefreshButton.BorderRadius = (int)(w / 2);

#if __UWP__
            // Accommodate for wide scrollbar
            x -= 12;
#endif
            AddSubview(RefreshButton, x, y, w, h);
		}

		public void Initialize(ZipData items)
		{
			this.items = items;
			Update(0);
		}

		public void Update(int index)
		{
			Field.Update(items.DecompressedFiles[index]);
		}
	}
    
    public class RefreshButton : BaseView
    {
        public EventHandler<EventArgs> Clicked;

        Image image;

        public ImageSource ImageSource
        {
            get { return image.Source; }
            set { image.Source = value; }
        }

        public RefreshButton()
        {
            image = new Image();

            TapGestureRecognizer recognizer = new TapGestureRecognizer();
            recognizer.Tapped += delegate
            {
                if (Clicked != null)
                {
                    Clicked(this, EventArgs.Empty);
                }
            };

            GestureRecognizers.Add(recognizer);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            double padding = Width / 10;

            double x = padding;
            double y = padding;
            double w = Width - 2 * padding;
            double h = Height - 2 * padding;

            AddSubview(image, x, y, w, h);
        }
    }
}
