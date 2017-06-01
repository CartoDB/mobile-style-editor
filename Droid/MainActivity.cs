
using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace mobile_style_editor.Droid
{
	[Activity(Icon = "@drawable/icon_app", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			LoadApplication(new EditorApplication());

			if (((int)Build.VERSION.SdkInt) > 21)
			{
				Window.SetStatusBarColor(Colors.CartoNavyLight.ToNativeColor());
			}
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			if (requestCode == DriveClientDroid.RequestCode_RESOLUTION)
			{
				if (resultCode == Result.Ok)
				{
					DriveClientDroid.Instance.Connect();
				}
			}
			else if (requestCode == DriveClientDroid.RequestCode_OPENER)
			{
				if (resultCode == Result.Ok)
				{
					var driveId = (Android.Gms.Drive.DriveId)data.GetParcelableExtra(DriveClientDroid.Response_DRIVEID);
					DriveClientDroid.Instance.Download(driveId);
				}
			}
		}

		public void SetIsLandscape(bool landcape)
		{
			if (landcape)
			{
				RequestedOrientation = ScreenOrientation.Landscape;
			}
			else
			{
				RequestedOrientation = ScreenOrientation.Portrait;
			}
		}

	}
}
