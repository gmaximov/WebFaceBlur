using System;
using System.IO;
using System.Threading.Tasks;

namespace WebFaceBlur
{
    public interface IHtmlProcessorAsync
    {
        Task<string> RunAsync(Uri uri);
    }
}