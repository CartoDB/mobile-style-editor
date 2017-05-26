
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

        public string Text { get { return Field.Text; } }

        public CSSEditorView()
        {
            Field = new EditorField();
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
