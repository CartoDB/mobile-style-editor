using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Carto.Core;

namespace mobile_style_editor
{
	public class DriveClientUWP
	{
		public static readonly DriveClientUWP Instance = new DriveClientUWP();

		public EventHandler<DownloadEventArgs> DownloadComplete { get; set; }
		public EventHandler<ListDownloadEventArgs> ListDownloadComplete { get; set; }

		public EventHandler<EventArgs> UploadComplete { get; set; }

		const string CLIENTID_KEY = "client_id";
		const string CLIENTSECRET_KEY = "client_secret";
		const string REFRESHTOKEN_KEY = "refresh_token";

		string ClientId, ClientSecret, RefreshToken, AccessToken;

		/* 
		 * Gets and stores Access Token used for later queries 
		 */
		public async Task<AuthenticationResponse> Authenticate()
		{
			string url = "https://accounts.google.com/o/oauth2/token";

			ParseKeysFromFile();

			string data =
				"client_id=" + ClientId +
				"&client_secret=" + ClientSecret +
				"&refresh_token=" + RefreshToken +
				"&grant_type=refresh_token";

			string contentType = "application/x-www-form-urlencoded";
			var content = new StringContent(data, Encoding.UTF8, contentType);

			using (var client = new HttpClient())
			{
				HttpResponseMessage response = await client.PostAsync(url, content);
				string result = await response.Content.ReadAsStringAsync();
				return AuthenticationResponse.FromString(result);
			}
		}

		string MimeType = "q=mimeType='application/zip'";

		public async Task<List<DriveFile>> DownloadFileList()
		{
			List<DriveFile> driveFiles = new List<DriveFile>();

			// TODO AccessToken lifespan is 60 minutes. Re-request when 60 minutes have passed
			if (AccessToken == null)
			{
				var response = await Authenticate();
				AccessToken = response.Token;
			}

			// Set mime time as application/zip so it would only return zip files
			string url = "https://www.googleapis.com/drive/v3/files?" + MimeType;

			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Add("GData-Version", "3.0");
				client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);

				string result = await client.GetStringAsync(url);

				Variant variant = Variant.FromString(result);

				Variant files = variant.GetObjectElement("files");

				for (int i = 0; i < files.ArraySize; i++)
				{
					Variant file = files.GetArrayElement(i);
					driveFiles.Add(DriveFile.FromVariant(file));
				}
			}

			return driveFiles;

			/*
			   {
				 "kind": "drive#fileList",
				 "incompleteSearch": false,
				 "files": [
					  {
					   "kind": "drive#file",
					   "id": "0By9YoP-GLAu8ZzlsM08yLUtIZkU",
					   "name": "bright-with-params-updated.zip",
					   "mimeType": "application/zip"
					  },
					  {
					   "kind": "drive#file",
					   "id": "0By9YoP-GLAu8WWdlVGZfemVZVzQ",
					   "name": "bright-cartocss-style-with-params.zip",
					   "mimeType": "application/zip"
					  },
					  {
					   "kind": "drive#file",
					   "id": "0By9YoP-GLAu8TktwcHk1X3VZZVk",
					   "name": "Magenta-Style.zip",
					   "mimeType": "application/zip"
					  },
					  {
					   "kind": "drive#file",
					   "id": "0By9YoP-GLAu8QkxlYTBSZDBEaUk",
					   "name": "blue-cartocss-style.zip",
					   "mimeType": "application/zip"
					  },
					  {
					   "kind": "drive#file",
					   "id": "0By9YoP-GLAu8LUhfQlVCQTNGMWc",
					   "name": "bright-cartocss-style.zip",
					   "mimeType": "application/zip"
					  }
				 ]
				}
			 */ 
		}

        public async Task<Stream> DownloadStyle(string id, string name)
        {
            string url = "https://drive.google.com/uc?export=download&id=";
            url += id;
            // TODO Can currently only download file that have been made shareable via Drive's web client
            
            // &confirm is used to suppress the download size warning.
            // Shouldn't be necessary since our styles aren't that large
            // url += "&confirm=";
            
            using (var client = new HttpClient())
            {
                //var stream = await client.GetStreamAsync(url);
                //return stream;

                var response = await client.GetAsync(url);
                
                var content = (StreamContent)response.Content;
                var message = await response.Content.ReadAsStringAsync();

                if (message.Length < 70000)
                {
                    // The approximate size of Google Drive login page
                    // The login page is downloaded when the style isn't a publicly available (sharing turned on)

                    // In this case, return null and handle it later
                    return null;
                }

                return await response.Content.ReadAsStreamAsync();
            }
        }

        public async void Upload(string name, Stream file)
        {

        }

        void ParseKeysFromFile()
		{
			using (var stream = new FileStream("Assets/drive_client_ids.json", FileMode.Open, FileAccess.Read))
			{
				using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
				{
					Variant data = Variant.FromString(reader.ReadToEnd());

					ClientId = data.GetObjectElement(CLIENTID_KEY).String;
					ClientSecret = data.GetObjectElement(CLIENTSECRET_KEY).String;
					RefreshToken = data.GetObjectElement(REFRESHTOKEN_KEY).String;
				}
			}
		}
	}

	public class AuthenticationResponse
	{
		const string ACCESSTOKEN = "access_token";

		public string Token { get; set; }

		public static AuthenticationResponse FromString(string result)
		{
			Variant variant = Variant.FromString(result);
			return new AuthenticationResponse { Token = variant.GetObjectElement(ACCESSTOKEN).String };
		}
	}
}
