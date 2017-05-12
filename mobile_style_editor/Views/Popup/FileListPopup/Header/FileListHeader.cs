using System;
using Xamarin.Forms;

namespace mobile_style_editor
{

	public class FileListHeader : BaseView
	{
		public BackButton BackButton { get; private set; }

		public Label Label { get; private set; }

		public SelectButton Select { get; private set; }

		public void OnBackPress()
		{
			string[] split = Text.Split('/');
			string last = split[split.Length - 1];

			if (last.Equals(""))
			{
				last = split[split.Length - 2];
			}

			Text = Text.Replace(last, "");
		}

		public string Text
		{
			get { return Label.Text; }
			set { Label.Text = value; }
		}

		public FileListHeader()
		{
			Label = new Label();
			Label.VerticalTextAlignment = TextAlignment.Center;
			Label.HorizontalTextAlignment = TextAlignment.Center;
			Label.TextColor = Color.White;
			Label.BackgroundColor = Colors.CartoNavy;
			Label.FontSize = 12;

			BackButton = new BackButton();

			Select = new SelectButton();
		}

		public override void LayoutSubviews()
		{
			double backSize = Height * 2;
			double selectSize = Height * 3;

			double x = 0;
			double y = 0;
			double h = Height;
			double w = backSize;

			AddSubview(BackButton, x, y, w, h);

			x += w;
			w = Width - (backSize + selectSize);

			AddSubview(Label, x, y, w, h);

			x += w;
			w = selectSize;

			AddSubview(Select, x, y, w, h);
		}

	}
}
