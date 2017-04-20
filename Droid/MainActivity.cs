
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace mobile_style_editor.Droid
{
	[Activity(Label = "mobile_style_editor.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Landscape)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			LoadApplication(new EditorApplication());
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			if (requestCode == DriveClient.RequestCode_RESOLUTION)
			{
				if (resultCode == Result.Ok)
				{
					DriveClient.Instance.Connect();
				}
			}
			else if (requestCode == DriveClient.RequestCode_OPENER)
			{
				if (resultCode == Result.Ok)
				{
					var driveId = (Android.Gms.Drive.DriveId)data.GetParcelableExtra(DriveClient.Response_DRIVEID);
					DriveClient.Instance.Download(driveId);
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
