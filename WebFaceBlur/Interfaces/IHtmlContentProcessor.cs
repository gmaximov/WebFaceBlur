using System;
using System.IO;
using System.Threading.Tasks;

namespace WebFaceBlur
{
    public interface IHtmlContentProcessor
    {
        Task<string> Process(string html, Uri uri);
    }
}