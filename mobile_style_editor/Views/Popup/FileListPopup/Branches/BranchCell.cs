
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class BranchCell : TextCell
	{
		public EventHandler<EventArgs> Click;

		public Octokit.Branch Branch { get; private set; }

		public BranchCell(Octokit.Branch branch)
		{
			Branch = branch;

			Text = branch.Name;
			TextColor = Colors.CartoNavy;

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
		}

		public static BranchCell FromBranch(Octokit.Branch branch)
		{
			return new BranchCell(branch);
		}
	}
}
