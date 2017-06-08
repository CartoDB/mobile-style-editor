using System;
using Xamarin.Forms;

namespace mobile_style_editor
{
    public class TestController : BaseController
    {
        public TestView ContentView { get; set; }

        public TestController()
        {
			NavigationPage.SetHasNavigationBar(this, false);

			ContentView = new TestView();

            ContentView.IsNavigationBarVisible = true;
            ContentView.NavigationBar.IsBackButtonVisible = false;
            ContentView.NavigationBar.Title.Text = "TEST CONTROLLER";

            Content = ContentView;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ContentView.Content.Click += PushController;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ContentView.Content.Click -= PushController;
        }

        public async void PushController(object sender, EventArgs e)
		{
			string path = System.IO.Path.Combine(Parser.ApplicationFolder, StyleListController.TemplateFolder);
			string name = "nutiteq-bright-blue-small.zip";
			await Navigation.PushAsync(new MainController(path, name));
		}

	}

    public class TestView : ContentView
    {
        public ClickView Content { get; set; }

        public TestView()
        {
            Content = new ClickView();
            Content.BackgroundColor = Colors.CartoGreen;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            double padding = 30;
            AddSubview(Content, padding, padding, Width - 2 * padding, Height - 2 * padding);
        }
    }
}
