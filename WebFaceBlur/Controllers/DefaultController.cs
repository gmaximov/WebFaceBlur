using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebFaceBlur.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public async Task<ActionResult> Index(string path)
        {
            try
            {
                path = HttpUtility.UrlDecode(path);

                Uri uri = null;
                //Uri.IsWellFormedUriString(path, UriKind.Absolute);

                if ( !Uri.TryCreate(path, UriKind.Absolute, out uri) && (!Uri.TryCreate("http://" + path, UriKind.Absolute, out uri)) )
                {
                    throw new Exception();
                }

                HttpClientWrapper httpClient = new HttpClientWrapper();
                             
                string contentType = string.Empty;

                try
                {
                    HttpContentHeaders headers = await httpClient.GetHeadersAsync(uri);
                    contentType = headers.ContentType.MediaType;
                }
                catch
                {
                    //write base64 part
                    throw;
                }

                if ( contentType == string.Empty )
                {
                    throw new Exception();
                }


                if ( contentType.Contains("html") )
                {
                    HtmlProcessor processor = new HtmlProcessor(httpClient, uri);

                    string contentHtml = await processor.RunAsync();

                    ViewBag.Content = contentHtml;
                    return View("Index");
                }
                else
                {
                    MemoryStream contentStream;

                    if ( contentType.Contains("image") )
                    {
                        MicrosoftFaceDetection faceDetection = new MicrosoftFaceDetection();
                        ImageCacheInMemory imageCache = new ImageCacheInMemory();

                        ImageProcessor processor = new ImageProcessor(httpClient, uri, faceDetection, imageCache);

                        contentStream = await processor.RunAsync();
                    }
                    else
                    {
                        DefaultProcessor processor = new DefaultProcessor(httpClient, uri);

                        contentStream = await processor.RunAsync();
                    }

                    contentStream.Position = 0;
                    return File(contentStream, contentType);
                }
            }
            catch
            {
                ViewBag.isError = true;
                return View();
            }
        }
    }
}