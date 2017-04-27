using System;
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

		public async void DownloadFileList()
		{
			// TODO AccessToken lifespan is 60 minutes. Re-request when 60 minutes have passed
			if (AccessToken == null)
			{
				var response = await Authenticate();
				AccessToken = response.Token;
			}

			string url = "https://www.googleapis.com/drive/v3/files?" + MimeType;

			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Add("GData-Version", "3.0");
				client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);

				string result = await client.GetStringAsync(url);
				Console.WriteLine(result);
			}
		}

		void ParseKeysFromFile()
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
