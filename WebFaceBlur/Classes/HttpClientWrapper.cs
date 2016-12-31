using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace WebFaceBlur
{
    public class HttpClientWrapper
    {
        public static HttpClient httpClient = new HttpClient();

        public static async Task<HttpResponseMessage> GetAsync(Uri uri)
        {
            return await GetAsync(uri.ToString());
        }
        public static async Task<HttpResponseMessage> GetAsync(string uri)
        {
            return await httpClient.GetAsync(uri);
        }

        public static async Task<string> GetContentType(Uri uri)
        {
            return await GetContentType(uri.ToString());
        }
        public static async Task<string> GetContentType(string uri)
        {
            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, uri));
            return response.Content.Headers.ContentType.MediaType;
        }
        
    }
}