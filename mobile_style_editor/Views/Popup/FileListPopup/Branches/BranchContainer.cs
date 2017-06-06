
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace mobile_style_editor
{
    public class BranchContainer : BaseView
    {
        public EventHandler<EventArgs> CellClick;

        public BranchHeader Header { get; private set; }

        TableSection header;
        TableView content;

        public double HeaderHeight { get; set; }
        public double TotalHeight { get; set; }
        public double OriginalY { get; set; }

        public new Color BackgroundColor
        {
            get { return content.BackgroundColor; }
            set { content.BackgroundColor = value; }
        }

        public BranchContainer()
        {
            header = new TableSection();

            content = new TableView();
            content.Root = new TableRoot();
            content.Root.Add(header);

            Header = new BranchHeader();

            Header.Click += delegate
            {
                ToggleHeight();
            };

			BackgroundColor = Color.FromRgb(240, 240, 240);
		}

        public override void LayoutSubviews()
        {
            AddSubview(Header, 0, 0, Width, HeaderHeight);
            AddSubview(content, 0, HeaderHeight, Width, Height - HeaderHeight);

            base.LayoutSubviews();
        }

        public void Add(IReadOnlyList<Octokit.Branch> branches)
        {
            Clear();

            foreach (var branch in branches)
            {
                var cell = BranchCell.FromBranch(branch);

                cell.Click += delegate {
                    UpdateText(cell.Branch.Name);
                    CellClick(cell, EventArgs.Empty);
                };

                header.Add(cell);
            }
        }

        void UpdateText(string text)
        {
            Header.Text = "BRANCH: " + text;
        }

        public void Clear()
        {
			header.Clear();
		}

        public bool IsExpanded { get { return Height.Equals(TotalHeight); } }

		void ToggleHeight()
		{
			if (IsExpanded)
			{
				CollapseBranches();
			}
			else
			{
				ExpandBranches();
			}
		}

		public void ExpandBranches()
		{
            UpdateLayout(OriginalY - (TotalHeight - HeaderHeight), TotalHeight);
		}

		public void CollapseBranches()
		{
            UpdateLayout(OriginalY, HeaderHeight);
		}
    }

    public class BranchHeader : ClickView
	{
		Label label;

        public string Text
        {
            get { return label.Text; } 
            set { label.Text = value; } 
        }

		public BranchHeader()
        {
			label = new Label();
			label.BackgroundColor = Colors.CartoNavy;
			label.Text = "BRANCH: master";
			label.FontSize = 12;
			label.TextColor = Color.White;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.HorizontalTextAlignment = TextAlignment.Center;

		}

        public override void LayoutSubviews()
        {
            AddSubview(label, 0, 0, Width,Height);
        }
    }
}
