using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class FileTabPopup : BaseView
	{
		public ZipData Data { get; private set; }

		public new bool IsVisible { get { return Opacity == 1; } }

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

		public double RowCount { get; set; }

		public override void LayoutSubviews()
		{
			if (items == null || items.Count == 0)
			{
				return;
			}

			double x = 0;
			double y = 0;
			double w = Width / 3;
			double h = Height / RowCount;

			foreach (var item in items)
			{
				item.UpdateLayout(x, y, w, h);

				int nextX = (int)Math.Ceiling(x + w);

				if (nextX <= (int)Width + 2 && nextX >= (int)Width - 2)
				{
					x = 0;
					y += h;
				}
				else
				{
					x += w;
				}
			}
		}

		public void Initialize(ZipData data)
		{
			Data = data;

			Opacity = 0;

            AddTabs(data);

			Highlight(0);
		}

		void AddTabs(ZipData data)
		{
			items = new List<FileTab>();

			Children.Clear();
			items.Clear();

			List<string> names = data.StyleFileNames;
			int index = 0;
			foreach (string name in names)
			{
				FileTab tab = new FileTab(name, index);
				AddSubview(tab);
				items.Add(tab);

				tab.Tapped += OnTap;

				index++;
			}

			LayoutSubviews();
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
			//base.IsVisible = true;
            this.FadeTo(1.0);
        }

        public async void Hide()
		{
            await this.FadeTo(0.0);
			//base.IsVisible = false;
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
