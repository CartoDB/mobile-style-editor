
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace mobile_style_editor
{
    public class BranchContainer : BaseView
    {
        public EventHandler<EventArgs> CellClick;

        Label label;

        TableSection header;
        TableView content;

        public double HeaderHeight { get; set; }

        public BranchContainer()
        {
            label = new Label();
            label.BackgroundColor = Colors.CartoNavy;
            label.Text = "BRANCHES";
            label.FontSize = 12;
            label.TextColor = Color.White;
            label.VerticalTextAlignment = TextAlignment.Center;
            label.HorizontalTextAlignment = TextAlignment.Center;

            header = new TableSection();

            content = new TableView();
            content.Root = new TableRoot();
            content.Root.Add(header);
        }

        public override void LayoutSubviews()
        {
            AddSubview(label, 0, 0, Width, HeaderHeight);
            AddSubview(content, 0, HeaderHeight, Width, Height - HeaderHeight);
        }

        public void Add(IReadOnlyList<Octokit.Branch> branches)
        {
            Clear();

            foreach (var branch in branches)
            {
                var cell = BranchCell.FromBranch(branch);

                cell.Click += delegate {
                    CellClick(cell, EventArgs.Empty);
                };

                header.Add(cell);
            }
        }

        public void Clear()
        {
			header.Clear();
		}
    }

    public class BranchCell : TextCell
	{
		public EventHandler<EventArgs> Click;

		public Octokit.Branch Branch { get; private set; }

        public BranchCell(Octokit.Branch branch)
        {
            Branch = branch;

            Text = branch.Name;

			TapGestureRecognizer recognizer = new TapGestureRecognizer();
			recognizer.Tapped += delegate
			{
				if (!IsEnabled)
				{
					return;
				}

				if (Click != null)
				{
					Click(this, EventArgs.Empty);
				}
			};
            Command = new Command((obj) =>
            {
				if (!IsEnabled)
				{
					return;
				}

				if (Click != null)
				{
					Click(this, EventArgs.Empty);
				}
            });
            //GestureRecognizers.Add(recognizer);
        }

        public static BranchCell FromBranch(Octokit.Branch branch)
        {
            return new BranchCell(branch);
        }
    }
}
