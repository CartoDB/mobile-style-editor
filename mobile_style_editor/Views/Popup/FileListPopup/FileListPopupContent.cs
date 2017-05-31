using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class FileListPopupContent : BasePopupContent
	{
		public EventHandler<EventArgs> ItemClick;

		public override void LayoutSubviews()
		{
			double padding = 0;

			int itemsPerRow = 7;

			if (!IsTablet)
			{
				itemsPerRow = 5;
			}

			double x = padding;
			double y = padding;
			double w = Width / itemsPerRow;
			double h = w * 1.2;

			foreach (var view in Children)
			{
				if (!(view is FileListPopupItem))
				{
					continue;
				}

				var item = (FileListPopupItem)view;

				item.UpdateLayout(x, y, w, h);

				if (Width - (x + w) < 1)
				{
					x = padding;
					y += h + padding;
				}
				else
				{
					x += w + padding;
				}
			}

		}

        Queue<FileListPopupItem> cache = new Queue<FileListPopupItem>();

		public void Populate(List<object> items)
		{
			/*
             * WOOP! 
             * 
             * This caching logic reduces average pagination speed from ~450 to ~350
             * (Tested on Android, Samsung Galaxy Tab S2)
             * Not as much as I'd hoped for, but it's something.
             * 
             * Updating layout in itself takes 100ms.
             * A view's construction takes ~10ms, fetching it from cache takes ~1ms
             * 
             * Using the same HubClient.PageSize (25) views each time, just changing the content, 
             * not even updating layout, and then hiding some if it's the final page
             * would somewhat improve performance, but is rather unnecessary
             *
             */

			Children.Clear();

            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

			foreach (object item in items)
			{
                FileListPopupItem view;

                if (cache.Count > 0)
                {
                    view = cache.Dequeue();
					Console.WriteLine("Loading from Cache: " + watch.ElapsedMilliseconds);
				}
                else
                {
                    view = new FileListPopupItem();
                    Console.WriteLine("Creating new instance: " + watch.ElapsedMilliseconds);
                }

                view.Reset();

				if (item is DriveFile)
				{
					view.DriveFile = (DriveFile)item;
				}
				else if (item is StoredStyle)
				{
					view.Style = (StoredStyle)item;
				}
				else
				{
					view.GithubFile = (GithubFile)item;
				}

				view.Initialize();

				view.Click += OnItemClick;

				AddSubview(view);

                Console.WriteLine("Subview added: " + watch.ElapsedMilliseconds);
			}

            foreach (var view in Children)
            {
                if (view is FileListPopupItem)
                {
                    cache.Enqueue(view as FileListPopupItem);
                }
            }

            Console.WriteLine("Before Layout: " + watch.ElapsedMilliseconds);

			LayoutSubviews();

			Console.WriteLine("Total: " + watch.ElapsedMilliseconds);
		}

		void OnItemClick(object sender, EventArgs e)
		{
			if (ItemClick != null)
			{
				ItemClick(sender, e);
			}
		}
	}

}
