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

        public async Task<Stream> GetStreamAsync(string path)
        {
            return await httpClient.GetStreamAsync(path);
        }

        public async Task<string> GetStringAsync(string path)
        {
            return await httpClient.GetStringAsync(path);
        }

        public async Task<HttpResponseMessage> GetHeadersAsync(string path)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Head, path);
            HttpResponseMessage response = await httpClient.SendAsync(request);
            return response;
        }
    }
}