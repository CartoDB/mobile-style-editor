using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Carto.Core;
using Foundation;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using UIKit;

namespace mobile_style_editor.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

			LoadApplication(new EditorApplication());

			//GoogleClient.Instance.Authenticate();
			//List<DriveFile> items = GoogleClient.Instance.GetStyleList();
			//GoogleClient.Instance.DownloadStyle(items[0].Id);

			return base.FinishedLaunching(app, options);
		}

	}
}
