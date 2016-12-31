using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebFaceBlur
{
    public class DefaultContentProcessor : IContentProcessor
    {
        public async Task<Stream> Process(Stream stream, Uri uri)
        {
            stream.Position = 0;
            return stream;
        }
    }
}