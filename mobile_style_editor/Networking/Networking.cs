using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace mobile_style_editor
{
    public static class Networking
    {
        public static async Task<Stream> GetStyle(string name)
        {
            string baseUrl = "https://nutifront.s3.amazonaws.com/";
            string folder = "style_editor_sample_styles/";
            string url = baseUrl + folder + name;

            using (var client = new HttpClient())
            {
                HttpResponseMessage httpResponse = await client.GetAsync(url);

                return await httpResponse.Content.ReadAsStreamAsync();
            }
        }
    }
}
