using System;
using System.IO;
using System.Threading.Tasks;

namespace WebFaceBlur.App.Processors.Html
{
    public interface IHtmlProcessorAsync
    {
        Task<string> RunAsync(string url, string controllerPath);
    }
}