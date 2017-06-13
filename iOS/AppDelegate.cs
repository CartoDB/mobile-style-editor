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

            return base.FinishedLaunching(app, options);
        }
	}

	//[Register("AppDelegate")]
	//public class AppDelegate : UIApplicationDelegate
	//{
	//	const string License = "XTUN3Q0ZFMk5uUEwwQXQyUm5jN3BDeHpDeWdrMGs0VVRBaFFocVQybmRXRkxFcDlhUDBKWWxCN04rRHkwRWc9PQoKYXBwVG9rZW49NDQyNTRiNjQtNTRlNS00Y2Y1LTgzZDMtZDA2ZTU1M2QzOGIzCmJ1bmRsZUlkZW50aWZpZXI9Y29tLmNhcnRvLmNhcnRvbWFwCm9ubGluZUxpY2Vuc2U9MQpwcm9kdWN0cz1zZGsteGFtYXJpbi1pb3MtNC4qCndhdGVybWFyaz1jdXN0b20K";
	//	const string HockeyId = "0d9f582ec2c348598d1712d640ee16c4";

	//	public override UIWindow Window { get; set; }

	//	public UINavigationController Controller { get; set; }

	//	public static nfloat NavigationBarHeight;
	//	public static nfloat StatusBarHeight;

	//	public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
	//	{
 //           Controller = new UINavigationController(new TestViewController());

	//		Window = new UIWindow(UIScreen.MainScreen.Bounds);

	//		Window.RootViewController = Controller;

	//		Window.MakeKeyAndVisible();
	//		return true;
	//	}
	//}

    //public class TestViewController : UIViewController
    //{
    //    public override void ViewDidLoad()
    //    {
    //        base.ViewDidLoad();

    //        string userAgent = "Mozilla/5.0 (Linux; Android 5.1.1; Nexus 5 Build/LMY48B; wv) AppleWebKit/537.36 " +
    //            "(KHTML, like Gecko) Version/4.0 Chrome/43.0.2357.65 Mobile Safari/537.36";

    //        // set default useragent
    //        NSDictionary dictionary = NSDictionary.FromObjectAndKey(NSObject.FromObject(userAgent), NSObject.FromObject("UserAgent"));
    //        NSUserDefaults.StandardUserDefaults.RegisterDefaults(dictionary);

    //        //var auth = new Xamarin.Auth.OAuth2Authenticator(
    //        //	clientId: "187546970168-78c8th23ie1969bp9dlqja82fvsisg65.apps.googleusercontent.com",
    //        //	clientSecret: "Rj4b1TcTJnPRjJ2qSwr38PPR",
    //        //	scope: "openid",
    //        //	authorizeUrl: new Uri("https://accounts.google.com/o/oauth2/auth"),
    //        //	redirectUrl: new Uri("myredirect:oob"),
    //        //	accessTokenUrl: new Uri("https://accounts.google.com/o/oauth2/token"),
    //        //	getUsernameAsync: null,
    //        //             isUsingNativeUI: true
    //        //);

    //        var auth = new Xamarin.Auth.OAuth2Authenticator
		  //  (
		  //      "187546970168-78c8th23ie1969bp9dlqja82fvsisg65.apps.googleusercontent.com",
		  //      "https://www.googleapis.com/auth/drive",
		  //      new Uri("https://accounts.google.com/o/oauth2/auth"),
		  //      new Uri("urn:ietf:wg:oauth:2.0:oob"),
    //            //new Uri("http://localhost"),
		  //      null,
		  //      isUsingNativeUI: true
		  //  );
            
    //        var controller = auth.GetUI();

    //        NavigationController.PresentViewController(controller, true, delegate { });
    //    }
    //}
}
