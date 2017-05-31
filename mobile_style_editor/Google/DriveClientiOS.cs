#if __ANDROID__
#elif __IOS__
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Carto.Core;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Upload;

namespace mobile_style_editor
{
	public class DriveClientiOS
	{
		public static DriveClientiOS Instance { get; set; } = new DriveClientiOS();

		public EventHandler<DownloadEventArgs> DownloadComplete { get; set; }
		public EventHandler<EventArgs> UploadComplete { get; set; }

		public EventHandler<ListDownloadEventArgs> ListDownloadComplete { get; set; }

		const string CLIENTID_KEY = "client_id";
		const string CLIENTSECRET_KEY = "client_secret";
		const string REFRESHTOKEN_KEY = "refresh_token";

		static string RefreshToken = "";
		static string ClientId = "";
		static string ClientSecret = "";

		/*
		 * (1) Register "Other" application to get both clientId and clientSecret
		 * Note that official guides are of no use, 
		 * i.e. https://developers.google.com/drive/v3/web/quickstart/dotnet sample just causes the application to freeze;
		 * Nothing is returned. Found alternative means:
		 * http://stackoverflow.com/questions/27573272/googlewebauthorizationbroker-authorizeasync-hangs
		 * 
		 * (2) To get correct authentication token, follow the guide at:
		 * http://stackoverflow.com/questions/5850287/youtube-api-single-user-scenario-with-oauth-uploading-videos/8876027#8876027
		 *
		 * Finally:
		 * Constructed.json file for in-house use. Contect aare@carto.com to get a hold of file drive_client_ids.json,
		 * otherwise you need to go through the entire application registration process
		 * 
		 * Milestone TODO (probably not worth it to implement in the near future):
		 * Allow the user to authenticate via basic webview, HTTPRequest to retrieve refresh_token after that
		 */

		/*
		 * Additional Notes:
		 * (1) For now, no additional errorhandling is done, meaning that
		 * we assume authentication is successful, so credentials and service will be available
		 */

		UserCredential credentials;

		DriveService Service
		{
			get
			{
				return new DriveService(new BaseClientService.Initializer()
				{
					HttpClientInitializer = credentials,
					/* ApplicationName doesn't necessarily have to be package id / bundle identifier */
					ApplicationName = "com.carto.style.editor",
				});
			}
		}

		public void Authenticate()
		{
			using (var stream = new FileStream("drive_client_ids.json", FileMode.Open, FileAccess.Read))
			{
				using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
				{
					Variant data = Variant.FromString(reader.ReadToEnd());

					ClientId = data.GetObjectElement(CLIENTID_KEY).String;
					ClientSecret = data.GetObjectElement(CLIENTSECRET_KEY).String;
					RefreshToken = data.GetObjectElement(REFRESHTOKEN_KEY).String;
				}
			}

			var secrets = new ClientSecrets() { ClientId = ClientId, ClientSecret = ClientSecret };
			var initializer = new GoogleAuthorizationCodeFlow.Initializer { ClientSecrets = secrets };
			var flow = new GoogleAuthorizationCodeFlow(initializer);

			var token = new TokenResponse { RefreshToken = RefreshToken };

			credentials = new UserCredential(flow, "user", token);
		}

        public About GetUserInfo()
        {
            var resource = Service.About.Get();
            resource.Fields = "storageQuota, user/displayName";
			var info = resource.Execute();

            Console.WriteLine(info);

            return info;
        }

		public void DownloadStyleList()
		{
			Task.Run(delegate
			{
				List<DriveFile> items = new List<DriveFile>();

				// Define parameters of request
				FilesResource.ListRequest listRequest = Service.Files.List();
				listRequest.Fields = "nextPageToken, files(id, name, trashed)";

				IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;

				/*
				 * The listRequest.Execute() retrieves all files and folders from Drive, recursively
				 * Filter out irrelevant data, the requirement of a style file is that it ends with "-style.zip"
				 */

				foreach (var file in files)
				{
					if (!((bool)file.Trashed) && file.Name.EndsWith(".zip", StringComparison.Ordinal))
					{
						items.Add(DriveFile.FromGoogleApiDriveFile(file));
					}
				}

				if (ListDownloadComplete != null)
				{
					ListDownloadComplete(null, new ListDownloadEventArgs { DriveFiles = items });
				}
			});
		}

		public void DownloadStyle(string id, string name)
		{
			Task.Run(delegate
			{
				FilesResource.GetRequest request = Service.Files.Get(id);

				var stream = new MemoryStream();

				request.MediaDownloader.ProgressChanged += (IDownloadProgress obj) =>
				{
					if (obj.Status == DownloadStatus.Completed)
					{
						if (DownloadComplete != null)
						{
							DownloadComplete(null, new DownloadEventArgs { Stream = stream, Name = name });
						}
					}
				};

				request.Download(stream);
			});
		}

		public void Upload(string name, MemoryStream stream)
		{
			stream.Seek(0, SeekOrigin.Begin);

			Google.Apis.Drive.v3.Data.File body = new Google.Apis.Drive.v3.Data.File();
			body.Name = name;

			FilesResource.CreateMediaUpload request = Service.Files.Create(body, stream, "application/zip");

			request.ProgressChanged += (IUploadProgress obj) =>
			{
				if (obj.Status == UploadStatus.Completed)
				{
					if (UploadComplete != null)
					{
						UploadComplete(name, null);
					}
				}
			};

			request.Upload();
		}
	}
}
#elif __UWP__
#endif
