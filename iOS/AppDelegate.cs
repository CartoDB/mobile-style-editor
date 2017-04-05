using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
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
}
