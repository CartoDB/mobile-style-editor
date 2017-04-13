﻿using System;
using System.Collections.Generic;
using System.Reflection;
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
#endif
		public EditorApplication()
		{
#if __IOS__
			MapView.RegisterLicense(CartoLicense);
#elif __ANDROID__
			MapView.RegisterLicense(CartoLicense, Forms.Context);
#endif

#if __ANDROID__
			MainPage = new NavigationPage(new PickerController());
#elif __IOS__
			List<string> data = CopyToAppData();
			string folder = data[1];
			string filename = data[0];
			MainPage = new NavigationPage(new MainController(folder, filename));
#endif
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}

		// TODO Remove when iOS has Drive integration

		public List<string> CopyToAppData()
		{
			string styleName = "bright-cartocss-style";
			string stylePath = "";

			Assembly assembly = Assembly.GetAssembly(typeof(Parser));
			string[] resources = assembly.GetManifestResourceNames();

			foreach (var resource in resources)
			{
				if (resource.Contains(styleName) && !resource.Contains("with-params"))
				{
					stylePath = resource;
				}
			}

			using (var stream = assembly.GetManifestResourceStream(stylePath))
			{
				string filename = styleName + Parser.ZipExtension;
				return FileUtils.SaveToAppFolder(stream, filename);
			}
		}
	}
}
