
using Carto.Ui;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class EditorApplication : Application
	{
#if __IOS__
		const string CartoLicense = "XTUN3Q0ZES3J1SEtaMDlxZ3REL1hpaW5PaXgxSmxOR3NBaFJxWmpyZTg0OEoxK21VRldjcUUrcmNTQWVhb2c9PQoKYXBwVG9rZW49MGE5MmIxMDQtMDY1OS00ZTU1LTg5ZDktNWUwNTA2NjhlMjY3CmJ1bmRsZUlkZW50aWZpZXI9Y29tLmNhcnRvLnN0eWxlLmVkaXRvcgpvbmxpbmVMaWNlbnNlPTEKcHJvZHVjdHM9c2RrLXhhbWFyaW4taW9zLTQuKgp3YXRlcm1hcms9Y3VzdG9tCg==";
#elif __ANDROID__
        const string CartoLicense = "XTUMwQ0ZRQ3NUWVVJbTFGdUdJcDd6VDhMTEJMaU4zTFhtUUlVVGEzaVljMld0TnVSOTMxV0FkNkx1WldOQ2lvPQoKYXBwVG9rZW49YTJmZjQ3NDYtODYyOC00ZTcyLWI1MzMtMmJjNGQwMGU1MzJkCnBhY2thZ2VOYW1lPWNvbS5jYXJ0by5zdHlsZS5lZGl0b3IKb25saW5lTGljZW5zZT0xCnByb2R1Y3RzPXNkay14YW1hcmluLWFuZHJvaWQtNC4qCndhdGVybWFyaz1jdXN0b20K";
#elif __UWP__
        const string CartoLicense = "XTUMwQ0ZEdHlmdlhhOGZoemNodFJTS0FPNUVlb3dLSVNBaFVBcFFRcjFyVHpBbkJJdG11YWV4WE92SDNoMllJPQoKYXBwVG9rZW49ZDY1Y2RmYzAtMGVkMC00MDJkLTgwOWEtN2RlNjNkNjBhMDgwCnByb2R1Y3RJZD0xZTcxYjA5NC1kMzY5LTQ0ZTMtYWEwZi1iZTg0NmUzZjZkN2QKcHJvZHVjdHM9c2RrLXdpbnBob25lLTQuKgpvbmxpbmVMaWNlbnNlPTEKd2F0ZXJtYXJrPWN1c3RvbQo=";
#endif

#if __IOS__
		const string HockeyId = "477fcd9fba6d4c29be7d0256dfffac7e";
#elif __ANDROID__
        const string HockeyId = "c005e5b5e4de4567a5bb6b82248005c4";
#elif __UWP__
#endif

		public static double Height { get { return NavigationPage.Height; } }

		public static double Width { get { return NavigationPage.Width; } }

		public static readonly NavigationPage NavigationPage = new NavigationPage(new StyleListController())
		{
			BarBackgroundColor = Colors.CartoNavy,
			BarTextColor = Color.White
		};

		public EditorApplication()
		{

#if __IOS__
			MapView.RegisterLicense(CartoLicense);
#elif __ANDROID__
            MapView.RegisterLicense(CartoLicense, Forms.Context);
#elif __UWP__
            MapView.RegisterLicense(CartoLicense);
#endif


#if __IOS__
			HockeyApp.iOS.BITHockeyManager manager = HockeyApp.iOS.BITHockeyManager.SharedHockeyManager;
			manager.Configure(HockeyId);
			manager.DisableUpdateManager = false;
			manager.StartManager();
#elif __ANDROID__
            HockeyApp.Android.CrashManager.Register(Forms.Context, HockeyId);

#elif __UWP__
#endif
			MainPage = NavigationPage;

			//throw new System.Exception();
		}

		protected override void OnStart()
		{

		}

		protected override void OnSleep()
		{

		}

		protected override void OnResume()
		{

		}

	}
}
