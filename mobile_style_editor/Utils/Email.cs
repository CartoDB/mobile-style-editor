
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

        public static void OpenSender(string path)
        {
#if __IOS__
            string[] split = path.Split('/');
            string name = split[split.Length - 1];

            var stream = new MemoryStream(FileUtils.PathToByteData(path));

            NSData data = NSData.FromStream(stream);

            var controller = new MFMailComposeViewController();
            controller.SetSubject("Style: " + name);
            controller.SetMessageBody("Date: " + DateTime.Now.ToString("F"), false);
            controller.AddAttachmentData(data, "application/zip", name);

            controller.Finished += (object sender, MFComposeResultEventArgs e) => {

                string message = "";

                if (e.Result == MFMailComposeResult.Sent) {
                    message = "Mail sent";
                } else if (e.Result == MFMailComposeResult.Failed) {
                    message = "Failed :" + e.Error;
                } else if (e.Result == MFMailComposeResult.Cancelled) {
                    message = "Cancelled";
                } else if (e.Result == MFMailComposeResult.Saved) {
                    message = "Saved";
                }

                Toast.Show(message, new BaseView());
                controller.DismissViewController(true, null);
            };
			
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
