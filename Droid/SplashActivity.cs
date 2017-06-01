
using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;

namespace mobile_style_editor.Droid
{
    [Activity(Theme = "@style/MyTheme.Splash", 
              MainLauncher = true,
              NoHistory = true)]
	public class SplashActivity : AppCompatActivity
	{
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            StartActivity(typeof(MainActivity));
        }

	}
}
