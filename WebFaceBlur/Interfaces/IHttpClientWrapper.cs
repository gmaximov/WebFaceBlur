using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebFaceBlur
{
    public interface IHttpClientWrapperAsync
    {
        Task<Stream> GetStreamAsync(Uri uri);
        Task<string> GetStringAsync(Uri uri);
        Task<HttpContentHeaders> GetHeadersAsync(Uri uri);
    }
}
