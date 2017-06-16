
using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class NavigationBar : BaseView
	{
        public static readonly double HEIGHT = 50;

        public EventHandler<EventArgs> EditingEnded;

		public bool IsBackButtonVisible { get; set; } = true;

        public bool IsTitleEditVisible { get; set; }

        public double BaseY
        {
            get
            {
                if (Device.RuntimePlatform.Equals("iOS")) { return 20; }
                return 0;
            }
        }

		public new double Height { get { return HEIGHT + BaseY; } }

		BaseView statusbar, container;

		public NavigationBackButton Back { get; private set; }

        public TitleEditButton Edit { get; private set; }

        public MeasuredLabel Title { get; private set; }

        BaseView rightItem;

		public NavigationBar()
		{
            Console.WriteLine(Device.RuntimePlatform);

			BackgroundColor = Colors.CartoNavyLight;

			statusbar = new BaseView { BackgroundColor = Colors.CartoNavyLight };
			container = new BaseView();

			Title = new MeasuredLabel();
			Title.TextColor = Color.White;
			Title.FontAttributes = FontAttributes.Bold;
			Title.VerticalTextAlignment = TextAlignment.Center;
			Title.HorizontalTextAlignment = TextAlignment.Center;
			Title.FontSize = 14;
            Title.NumberOfLines = 1;

			field = new BaseEntry();
			field.BackgroundColor = Colors.CartoNavyLight;
			field.TextColor = Title.TextColor;
			field.FontSize = Title.FontSize;
			field.AutoCorrectEnabled = false;
            field.FontAttributes = Title.FontAttributes;
            field.IsVisible = false;

			Back = new NavigationBackButton();

            Edit = new TitleEditButton();

			Title.Measured += delegate
			{
                double x = Width / 2 - Title.MeasuredWidth / 2;

                Title.UpdateLayout(x, BaseY, Title.MeasuredWidth, Height - BaseY);
                Edit.UpdateX(x + Title.MeasuredWidth);
			};
		}

        public override void LayoutSubviews()
        {
            double statusbarHeight = BaseY;

            double x = 0;
            double y = 0;
            double w = Width;
            double h = statusbarHeight;

            AddSubview(statusbar, x, y, w, h);

            y += h;
            h = Height - statusbarHeight;

            AddSubview(container, x, y, w, h);

            x = 0;
            y = 0;

            if (IsBackButtonVisible)
            {
                w = h * 2;
                container.AddSubview(Back, x, y, w, h);
            }
            else
            {
                w = h;
            }

            w = Title.MeasuredWidth;
            h = Title.MeasuredHeight;
            x = w;
            y = Height / 2 - h / 2 + BaseY;

            AddSubview(Title, x, y, w, h);

            if (IsTitleEditVisible)
            {
                x = Title.X + Title.Width;
                y = BaseY;
                w = Height / 2;
                h = w;

                AddSubview(Edit, x, y, w, h);
            }

            if (rightItem != null)
            {
                h = Height - statusbarHeight;
                w = h;
                x = Width - w;
                y = BaseY;

                AddSubview(rightItem, x, y, w, h);
            }

            double extra = Title.Width / 3;
            x = Title.X - extra;
            w = Title.Width + (2 * extra);
            AddSubview(field, x, Title.Y, w, Title.Height);
        }

		public void Add(BaseView parent, double width)
		{
			parent.AddSubview(parent, 0, BaseY, width, Height);
		}

        public void AddRightBarButton(BaseView view)
        {
            rightItem = view;
        }

        public void AttachHandlers()
        {
			field.Completed += OnEditingCompleted;
			field.Unfocused += CloseTitleEditor;
        }

        public void DetachHandlers()
        {
			field.Completed -= OnEditingCompleted;
			field.Unfocused -= CloseTitleEditor;
        }

        void ShowTitleField(bool value)
        {
            // I have no idea why this is necessary,
            // but we have to raise Title view, else it'll stay hidden... somewhere

            if (value) 
            {
                RaiseChild(Title);
            }

            field.IsVisible = !value;
            Edit.IsVisible = value;
            Title.IsVisible = value;
        }

        BaseEntry field;
        System.Timers.Timer focusTimer;
        bool didJustFocus;

        public void OpenTitleEditor()
        {
            ShowTitleField(false);

            didJustFocus = true;

            field.Focus();

            focusTimer = new System.Timers.Timer(100);
            focusTimer.Start();

            focusTimer.Elapsed += delegate
            {
                didJustFocus = false;

                focusTimer.Stop();
                focusTimer.Dispose();
                focusTimer = null;
            };
        }

        public void CloseTitleEditor()
        {
            ShowTitleField(true);
        }

		void OnEditingCompleted(object sender, EventArgs e)
        {
			EditingEnded?.Invoke(field.Text, EventArgs.Empty);
        }

        void CloseTitleEditor(object sender, FocusEventArgs e)
        {
            if (didJustFocus)
            {
                /*
                 * Xamarin forms bug. 
                 * Seemingly randomly, sometimes Unfocused is called immediately after focus. 
                 * But not always. 
                 * God damnit. 
                 * Implemented special timer to check whether focus happened within 100ms or not.
                 */
                field.Focus();
                return;
            }

            CloseTitleEditor();
        }

        public void Revert()
        {
			field.Text = "";
		}

        public void UpdateText(string text)
        {
            Title.Text = text;

            field.Text = "";
        }
	}

}
