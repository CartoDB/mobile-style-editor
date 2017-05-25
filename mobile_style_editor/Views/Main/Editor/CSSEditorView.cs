
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

#if __ANDROID__
			Field.RemoveFromParent();
#elif __UWP__
            if (Field.Parent != null)
            {
                (Field.Parent as NativeViewWrapperRenderer).Children.Remove(Field);
            }
#endif
            AddSubview(Field.ToView(), x, y, w, h);

            double padding = 10;

            w = 50;
            h = w;
            x = Width - (w + padding);
            y = Height - (h + padding);

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

	public class RefreshButton : Frame
	{
		public ContainerView container;
		public EventHandler<EventArgs> Clicked;

		public ImageSource ImageSource
		{
			get { return container.image.Source; }
			set { container.image.Source = value; }
		}

		public RefreshButton()
		{
			TapGestureRecognizer recognizer = new TapGestureRecognizer();
			recognizer.Tapped += delegate
			{
				if (Clicked != null)
				{
					Clicked(this, EventArgs.Empty);
				}
			};

			Padding = new Thickness(0, 0, 0, 0);

			GestureRecognizers.Add(recognizer);

			container = new ContainerView();
			container.BackgroundColor = Colors.CartoRed;
			Content = container;

			SizeChanged += OnSizeChange;
		}

		void OnSizeChange(object sender, EventArgs e)
		{
#if __UWP__
			CornerRadius = (float)Width / 2;
#else
			// TODO Why does CornerRadius only exist on UWP (and doesn't even work there)?
#endif
		}
    }

    public class ContainerView : BaseView
    {
        public Image image;

        public ContainerView()
        {
            image = new Image();
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
