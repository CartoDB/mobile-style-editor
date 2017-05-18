
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class PaginationView : BaseView
	{
		public EventHandler<EventArgs> PageClicked;

		List<PageView> pages = new List<PageView>();

		public PaginationView()
		{
			BackgroundColor = Color.Transparent;
		}

		public override void LayoutSubviews()
		{
			double padding = 2;

			double x = 0;
			double y = 0;
			double w = 30;
			double h = 30;

			foreach (PageView page in pages)
			{
				AddSubview(page, x, y, w, h);
				x += w + padding;
			}
		}

		public void HighlightFirst()
		{
			pages[0].Highlight();
			currentHighlight = pages[0];
		}

		public void AddPage(List<GithubFile> files, int number)
		{
			PageView page = new PageView();
			page.GithubFiles = files;
			page.Text = number.ToString();
			page.Click += (object sender, EventArgs e) =>
			{
				OnPageClick(sender, e);
			};

			pages.Add(page);
			LayoutSubviews();
		}

		PageView currentHighlight;

		void OnPageClick(object sender, EventArgs e)
		{
			if (currentHighlight != null)
			{
				currentHighlight.Normalize();
			}

			var page = (PageView)sender;
			page.Highlight();
			currentHighlight = page;

			if (PageClicked != null)
			{
				PageClicked(sender, e);	
			}
		}

		public void Reset()
		{
			pages.Clear();
			Children.Clear();
		}

		public void Show(bool animated = true)
		{
			IsVisible = true;

			if (!animated)
			{
				Opacity = 1;
			}
			else
			{
				this.FadeTo(1);
			}
		}

		public virtual async void Hide(bool animated = true)
		{
			if (!animated)
			{
				Opacity = 0;
			}
			else
			{
				await this.FadeTo(0);
			}

			IsVisible = false;
		}
	}
}
