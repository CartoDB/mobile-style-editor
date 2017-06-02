
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
}
