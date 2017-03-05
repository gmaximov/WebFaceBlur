using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebFaceBlur.App.Http
{
    public interface IHttpClientWrapperAsync
    {
        Task<Stream> GetStreamAsync(string path);
        Task<string> GetStringAsync(string path);
        Task<HttpResponseMessage> GetHeadersAsync(string path);
    }
}
