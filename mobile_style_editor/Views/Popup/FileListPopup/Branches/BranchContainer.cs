
using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<BranchCell> Cells
        {
            get { return header.Where(cell => cell is BranchCell).Cast<BranchCell>().ToList(); }
        }

        public void Highlight(string branch)
        {
            Normalize();

            foreach (var cell in Cells)
            {
                if (cell.Branch.Name.Equals(branch))
                {
                    cell.Highlight();
                }
            }
        }

		public void Normalize()
		{
			foreach (var cell in Cells)
			{
                cell.Normalize();
			}
		}

        public void Add(IReadOnlyList<Octokit.Branch> branches)
        {
            Clear();

            foreach (var branch in branches)
            {
                var cell = BranchCell.FromBranch(branch);

                cell.Click += delegate {
                    
                    Normalize();
                    UpdateText(cell.Branch.Name);
                    cell.Highlight();

                    if (CellClick != null)
                    {
                        CellClick(cell, EventArgs.Empty);
                    }
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
}
