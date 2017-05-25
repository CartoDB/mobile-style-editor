
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class FileTabs : BaseView
	{
		public int ActiveIndex 
		{
			get
			{
				foreach (FileTab tab in items)
				{
					if (tab.IsHighlighted)
					{
						return tab.Index;
					}
				}

				return -1;
			}
		}

		public EventHandler OnTabTap;

		List<FileTab> items;

		public FileTabs()
		{
			items = new List<FileTab>();
		}

		public void Update(ZipData data)
		{
			Children.Clear();
			items.Clear();

			double x = 0;
			double y = 0;
            double w = 80;
			//double h = Height / 4;
			double h = Height;

			List<string> names = data.StyleFileNames;
			int index = 0;

			foreach (string name in names)
			{
				FileTab tab = new FileTab(name, index);
				AddSubview(tab, x, y, w, h);
				items.Add(tab);

				tab.Tapped += OnTap;

				if ((int)Math.Ceiling(x + w) == (int)Width)
				{
					x = 0;
					y += h;
				}
				else
				{
					x += w;
				}

				index++;
			}

            Highlight(0);
		}

		void OnTap(object sender, EventArgs e)
		{
			FileTab tab = (FileTab)sender;
			Highlight(tab);

			if (OnTabTap != null)
			{
				OnTabTap(tab, e);
			}
		}

		FileTab currentHighlight;

		public void Highlight(FileTab tab)
		{
			if (currentHighlight != null)
			{
				currentHighlight.Normalize();
			}

			tab.Highlight();
			currentHighlight = tab;
		}

		public void Highlight(int index)
		{
			if (currentHighlight != null)
			{
				currentHighlight.Normalize();
			}

			FileTab item = items[index];

			item.Highlight();
			currentHighlight = item;
		}
	}
}
