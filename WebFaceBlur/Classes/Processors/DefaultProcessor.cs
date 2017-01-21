using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebFaceBlur
{
    public class DefaultProcessor : IDefaultProcessorAsync
    {
        private IHttpClientWrapperAsync httpClient;
        private Uri uri;

        public DefaultProcessor(IHttpClientWrapperAsync httpClient, Uri uri)
        {
            this.httpClient = httpClient;
            this.uri = uri;
        }

        public async Task<MemoryStream> RunAsync()
        {
            MemoryStream memoryStream = new MemoryStream();
            using ( Stream stream = await httpClient.GetStreamAsync(uri) )
            {
                await stream.CopyToAsync(memoryStream);
            }

            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}