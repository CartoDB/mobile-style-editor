using System;
using System.Timers;

namespace mobile_style_editor
{
    public class SaveTimer
    {
        /*
         * Separate entity that updates data in volatile memory once every two seconds
         */
        const double Interval = 2 * 1000;

        static Timer timer;

        MainView contentView;

        public void Initialize(MainView contentView)
        {
            timer = new Timer(Interval);
            timer.Elapsed += OnElapsed;

			this.contentView = contentView;

            timer.Start();
        }

        public void Dispose()
        {
			timer.Dispose();
			timer = null;

            contentView = null;
        }

        void OnElapsed(object sender, ElapsedEventArgs e)
        {
            int index = contentView.ActiveIndex;

            string text = null;

            Xamarin.Forms.Device.BeginInvokeOnMainThread(delegate {
                text = contentView.Editor.Text;

                System.Threading.Tasks.Task.Run(delegate {

					if (text == null)
					{
						return;
					}

					contentView.Data.DecompressedFiles[index] = text;

                });
            });

        }
    }
}
