using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebFaceBlur
{
    public class Processor
    {
        public async static Task<string> Html(string html, Uri uri)
        {
            return await (new HtmlContentProcessor()).Process(html, uri);
        }

        public async static Task<Stream> ImageFile(Stream stream, Uri uri)
        {
            return await (new ImageFileContentProcessor()).Process(stream, uri);
        }

        public async static Task<Stream> Default(Stream stream, Uri uri)
        {
            return await (new DefaultContentProcessor()).Process(stream, uri);
        }
    }
}