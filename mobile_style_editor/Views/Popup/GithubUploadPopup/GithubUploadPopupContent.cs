
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class GithubUploadPopupContent : BasePopupContent
	{
        public BranchContainer Branches { get; private set; }

        public EntryWithLabel Comment { get; private set; }
		
        public Button Commit { get; private set; }

		public GithubUploadPopupContent()
		{
            Comment = new EntryWithLabel();

			Commit = new Button();
			Commit.Text = "COMMIT";
			Commit.BackgroundColor = Colors.CartoRed;
			Commit.TextColor = Color.White;

            Branches = new BranchContainer();
            Branches.BackgroundColor = Color.White;
		}

		public override void LayoutSubviews()
		{
			double padding = 10;

			double w = 200;
            double h = 400;
            double x = Width - (w + padding);
            double y = padding;

            AddSubview(Branches, x, y, w, h);

            x = padding;
            y = padding;
            w = Width - (w + 3 * padding);
            h = 70;

            AddSubview(Comment, x, y, w, h);

            w = 150;
            h = w / 4;
            x = Width - (w + padding);
            y = Height - (h + padding);

            AddSubview(Commit, x, y, w, h);
		}

        public void ShowBranches(IReadOnlyList<Octokit.Branch> branches)
        {
            Branches.Add(branches);
        }

        public void HighlightBranch(string branch)
        {
            Branches.Highlight(branch);
        }

        public void ShowBranchLoading()
        {
            Branches.ShowLoading();
        }

        public void HideBranchLoading()
        {
            Branches.HideLoading();
        }

    }

    public class EntryWithLabel : BaseView
    {
		Label nameLabel;
		Entry nameInput;

        public string Text
        {
            get { return nameInput.Text; }
            set { nameInput.Text = value; }
        }

        public EntryWithLabel()
        {
			nameLabel = new Label();
			nameLabel.VerticalTextAlignment = TextAlignment.End;
			nameLabel.Text = "COMMENT";
			nameLabel.TextColor = Color.Gray;
			nameLabel.FontSize = 12f;

			nameInput = new Entry();
			nameInput.FontSize = 13f;
			nameInput.TextColor = Colors.CartoNavy;
		}

        public override void LayoutSubviews()
        {
            double padding = 5;

            double x = padding;
            double y = 0;
            double w = Width - 2 * padding;
            double h = Height / 4;

            AddSubview(nameLabel, x, y, w, h);

            y += h;
            h = Height - h;

            AddSubview(nameInput, x, y, w, h);
        }
	}
}
