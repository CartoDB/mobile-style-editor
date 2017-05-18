using System;
using System.IO;
using System.Net.Http;
using System.Text;
using Carto.Core;

namespace mobile_style_editor
{
	public class DriveClient
	{
		const string CLIENTID_KEY = "client_id";
		const string CLIENTSECRET_KEY = "client_secret";

		public static readonly DriveClient Instance = new DriveClient();

		readonly string ClientId, ClientSecret;
		//string RefreshToken, AccessToken;

		DriveClient()
		{
			string folder = "";

#if __UWP__
			folder += "Assets/"
#endif
			using (var stream = new FileStream(folder + "drive_client_ids.json", FileMode.Open, FileAccess.Read))
			{
				using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
				{
					Variant data = Variant.FromString(reader.ReadToEnd());

					ClientId = data.GetObjectElement(CLIENTID_KEY).String;
					ClientSecret = data.GetObjectElement(CLIENTSECRET_KEY).String;
				}
			}
		}

		/*
		 * The following is the basic authentication page url, open in your webview
		 */
		public string AuthorizationUrl
		{
			get
			{
				/*
				 * Followed guide at: http://www.daimto.com/google-authentication-with-curl/#Requesting_Authorization
				 */

				string baseUrl = "https://accounts.google.com/o/oauth2/auth";

				var url = baseUrl;
				url += "?client_id=" + ClientId;
				url += "&redirect_uri=urn:ietf:wg:oauth:2.0:oob";
				url += "&scope=https://www.googleapis.com/auth/drive";
				url += "&response_type=code";

				return url;
			}
		}

		public async void Authenticate(string code)
		{
			// TODO use code that is received from Webview, this is a one-time code for testing
			code = "4/Ej--Cc4XFIzzXcDcm19XyFRsxAzIh3HY8xlwj9dX-JM";

			string url = "https://accounts.google.com/o/oauth2/token";

			string data = "code=" + code;
			data += "&client_id=" + ClientId;
			data += "&client_secret=" + ClientSecret;
			data += "&redirect_uri=urn:ietf:wg:oauth:2.0:oob";
			data += "&grant_type=authorization_code";

			string contentType = "application/x-www-form-urlencoded";
			var content = new StringContent(data, Encoding.UTF8, contentType);

			using (var client = new HttpClient())
			{
				HttpResponseMessage response = await client.PostAsync(url, content);
				string result = await response.Content.ReadAsStringAsync();
				Console.WriteLine(result);
			}
		}

	}
}
