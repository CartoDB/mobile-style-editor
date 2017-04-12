using System;
using System.Collections.Generic;

#if __ANDROID__
using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Drive;
using Android.Gms.Drive.Query;
using Android.OS;
using Android.Runtime;
using Java.Lang;
#elif __IOS__
#endif

namespace mobile_style_editor
{
#if __ANDROID__
	public class DriveClient : Java.Lang.Object, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
	{
		/*
		 * Requires activity reference. Thankfully, Xamarin.Forms.Forms.Context is the default MainActivity,
		 * when allowed, the activity's OnActivityResult will be called
		 * 
		 * Be sure to use the correct keystore's SHA1 when registering a client id,
		 * else it'll just return "Canceled" (0) in OnActivityResult without any additional error message.
		 * cf. https://developer.xamarin.com/guides/android/deployment,_testing,_and_metrics/MD5_SHA1/#OSX for Xamarin defaults
		 * 
		 * 
		 */

		public static DriveClient Instance = new DriveClient();

		Context context;

		GoogleApiClient client;

		public bool IsConnecting { get { return client.IsConnecting; } }

		public ResultCallback Result { get; private set; }

		public void Register(Context context)
		{
			this.context = context;

			GoogleApiClient.Builder builder = new GoogleApiClient.Builder(context, this, this);
			builder.AddApi(DriveClass.API);
			builder.AddScope(DriveClass.ScopeFile);

			client = builder.Build();
		}

		public void Connect()
		{
			client.Connect();
		}

		public void OnConnected(Bundle connectionHint)
		{
			Query();
		}

		public async void Query()
		{
			QueryClass query = new QueryClass.Builder().Build();

			// Method 1 for getting Drive files/folders
			Result = new ResultCallback();
			DriveClass.DriveApi.Query(client, query).SetResultCallback(Result);

			// Method 2 for getting Drive files/folders
			IDriveFolder folder = DriveClass.DriveApi.GetRootFolder(client);
			IDriveApiMetadataBufferResult result1 = (await folder.ListChildren(client)).JavaCast<IDriveApiMetadataBufferResult>();
			IEnumerator<Metadata> list1 = result1.MetadataBuffer.GetEnumerator();

			while (list1.MoveNext())
			{
				Metadata current = list1.Current;
				Console.WriteLine(current);
			}

			// Method 3 for getting Drive files/folders
			IDriveApiMetadataBufferResult result2 = await DriveClass.DriveApi.QueryAsync(client, query);
			IEnumerator<Metadata> list2 = result2.MetadataBuffer.GetEnumerator();

			while (list2.MoveNext())
			{
				Metadata current = list2.Current;
				Console.WriteLine(current);
			}
		}

		public const int RequestCode_RESOLUTION = 1;

		public void OnConnectionFailed(ConnectionResult result)
		{
			if (result.HasResolution)
			{
				try
				{
					result.StartResolutionForResult((Activity)context, RequestCode_RESOLUTION);
				}
				catch (IntentSender.SendIntentException e)
				{
					Console.WriteLine("Failed to start resolution result: " + e.Message);
				}
			}
			else
			{
				Console.WriteLine("Failed without resolution");
			}
		}

		public void OnConnectionSuspended(int cause)
		{
			throw new NotImplementedException();
		}
	}

	public class ResultCallback : Java.Lang.Object, IResultCallback
	{
		public void OnResult(Java.Lang.Object result)
		{
			IDriveApiMetadataBufferResult parsed = result.JavaCast<IDriveApiMetadataBufferResult>();
			Console.WriteLine(parsed);
		}
	}

#elif __IOS__

#endif
}
