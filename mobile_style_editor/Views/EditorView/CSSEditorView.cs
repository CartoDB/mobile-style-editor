
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

		public Button SaveButton { get; private set; }

		public string Text { get { return Field.Text; } }

		public CSSEditorView()
		{
			Field = new EditorField();

			SaveButton = new Button();
			SaveButton.BackgroundColor = Color.Red;
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

			AddSubview(SaveButton, x, y, w, h);
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
