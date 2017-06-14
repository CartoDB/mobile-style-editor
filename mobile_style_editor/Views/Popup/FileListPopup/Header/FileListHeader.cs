using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class FileListHeader : BaseView
	{
		public BackButton BackButton { get; private set; }

		public Label Label { get; private set; }

		public void OnBackPress()
		{
			string[] split = Text.Split('/');
            string last = GetLast(split, 1);

			Text = Text.Replace(last, "");

            Text = Text.Replace("//", "/");
		}

        string GetLast(string[] split, int counter, string last = null)
        {
            last = split[split.Length - counter];

            if (last.Equals("")) {
                counter = counter + 1;
                return GetLast(split, counter, last);
            }

            return last;
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

			var recognizer = new TapGestureRecognizer();
			recognizer.Tapped += delegate {
				// Do nothing, just to catch clicks and not close the popup
			};

			Label.GestureRecognizers.Add(recognizer);
		}

		public override void LayoutSubviews()
		{
			double x = 0;
			double y = 0;
			double h = Height;
            double w = Width;

            AddSubview(Label, x, y, w, h);

            w = Height * 2;

            AddSubview(BackButton, x, y, w, h);
		}

        public void ShowButtons()
        {
            BackButton.IsVisible = true;

            BackgroundColor = Color.Transparent;
        }

        public void HideButtons()
        {
            BackButton.IsVisible = false;

            BackgroundColor = Colors.CartoNavy;
        }
	}
}
