using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class FileTabPopup : BaseView
	{
		public new bool IsVisible {
            get
            {
#if __UWP__
                return Opacity == 1;
#else
                return TranslationY > (visibleY - 0.1);
#endif
            }
        }

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

		public FileTabPopup()
		{
			BackgroundColor = Colors.CartoNavy;
		}

		double visibleY;

		public void Initialize(MainView parent, ZipData data)
		{
			double rowCount = Math.Ceiling((double)data.DecompressedFiles.Count / 3);
			double rowHeight = parent.Toolbar.Height;

			double h = rowCount * rowHeight;

			double w = parent.Width / 3;
			double x = 0;
			double y = 0;

			visibleY = rowHeight;

			parent.AddSubview(this, x, y, w, h);

            /* Initially hide the popup without animation */
#if __UWP__
            this.FadeTo(0.0, 0);
            this.TranslateTo(0, visibleY, 0);
#else
            this.TranslateTo(0, -Height, 0);
#endif
            AddTabs(data);

			Highlight(0);
		}

		void AddTabs(ZipData data)
		{
			items = new List<FileTab>();

			Children.Clear();
			items.Clear();

			double x = 0;
			double y = 0;
			double w = Width / 3;
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
		}

		public bool Toggle()
		{
			if (IsVisible)
			{
				Hide();
				return false;
			}
			else
			{
				Show();
				return true;
			}
		}

		public void Show()
		{
#if __UWP__
            this.FadeTo(1.0);
#else
            this.TranslateTo(0, visibleY);
#endif

        }

        public void Hide()
		{
#if __UWP__
            this.FadeTo(0.0);
#else
            this.TranslateTo(0, -Height);
#endif
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

		public string CurrentHighlight { get { return currentHighlight.Text; } }

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
