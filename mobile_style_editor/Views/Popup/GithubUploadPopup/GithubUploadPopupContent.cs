
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class GithubUploadPopupContent : BasePopupContent
	{
        LabelWithTitle locationLabel;

        public EntryWithTitle Comment { get; private set; }
		
        public Button Commit { get; private set; }

		public GithubUploadPopupContent()
		{
            locationLabel = new LabelWithTitle("STYLE LOCATION");

            Comment = new EntryWithTitle("COMMENT");

			Commit = new Button();
			Commit.Text = "COMMIT";
			Commit.BackgroundColor = Colors.CartoRed;
			Commit.TextColor = Color.White;
		}

		public override void LayoutSubviews()
		{
			double padding = 10;

            double x = padding;
            double y = padding;
            double w = Width - 2 * padding;
            double h = 50;

            AddSubview(locationLabel, x, y, w, h);

            y += h + padding;
            h = 70;

            AddSubview(Comment, x, y, w, h);

            w = 150;
            h = w / 4;
            x = Width - (w + padding);
            y = Height - (h + padding);

            AddSubview(Commit, x, y, w, h);
		}

        public void Update(string owner, string repository, string path, string branch)
        {
            locationLabel.Text = owner + "/" + repository + "/" + path + " (branch: " + branch + ")";
        }

    }
}
