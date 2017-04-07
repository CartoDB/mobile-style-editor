
using System;
using Xamarin.Forms;

#if __IOS__
using Xamarin.Forms.Platform.iOS;
#elif __ANDROID__
using Xamarin.Forms.Platform.Android;
#endif

namespace mobile_style_editor
{
	public class CSSEditorView : BaseView
	{
		ZipData items;

		public EditorField Field { get; private set; }

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
}
