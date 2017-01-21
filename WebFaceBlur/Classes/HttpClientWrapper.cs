using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace WebFaceBlur
{
    public class HttpClientWrapper : IHttpClientWrapperAsync
    {
        public HttpClient httpClient;

        public HttpClientWrapper()
        {
            httpClient = new HttpClient();
        }

        public async Task<Stream> GetStreamAsync(Uri uri)
        {
            return await httpClient.GetStreamAsync(uri);
        }

        public async Task<string> GetStringAsync(Uri uri)
        {
            return await httpClient.GetStringAsync(uri);
        }

        public async Task<HttpContentHeaders> GetHeadersAsync(Uri uri)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Head, uri);
            HttpResponseMessage response = await httpClient.SendAsync(request);
            return response.Content.Headers;
        }
    }
}