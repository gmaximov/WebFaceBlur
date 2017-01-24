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

        public DefaultProcessor(IHttpClientWrapperAsync httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<MemoryStream> RunAsync(Uri uri)
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