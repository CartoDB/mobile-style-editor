
using System;
using System.IO;

#if __IOS__
using UIKit;
using Foundation;
using MessageUI;
#elif __ANDROID__
using Android.Content;
#endif

namespace mobile_style_editor
{
    public class Email
    {
#if __IOS__
        public static UIViewController Controller { get { return UIApplication.SharedApplication.KeyWindow.RootViewController; }}
#elif __ANDROID__
        public static Context Context { get { return Xamarin.Forms.Forms.Context; } }
#endif

        public static void Send(string path)
        {
#if __IOS__
            NSData data = NSData.FromStream(new MemoryStream(FileUtils.PathToByteData(path)));

            var controller = new MFMailComposeViewController();
            controller.SetSubject("subject");
            controller.SetMessageBody("body", false);
            controller.AddAttachmentData(data, "application/zip", "filename.zip");

            Controller.PresentViewController(controller, true, null);

#elif __ANDROID__
            Intent intent = new Intent(Intent.ActionSend);
            intent.SetType("application/zip");

            intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse(path));

            Context.StartActivity(Intent.CreateChooser(intent, "Share zip"));
#endif
		}

    }
}
