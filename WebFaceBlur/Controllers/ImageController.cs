using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebFaceBlur.App.Http;
using WebFaceBlur.App.Processors.Image;

namespace WebFaceBlur.Controllers
{
    public class ImageController : Controller
    {
        // GET: Image
        [OutputCache(VaryByParam = "*", Duration = 240, Location = OutputCacheLocation.Downstream)]
        public async Task<ActionResult> Get(string path)
        {
            if ( path == null )
            {
                return null;
            }

            path = HttpUtility.UrlDecode(path);

            HttpClientWrapper httpClient = new HttpClientWrapper();

            string contentType = string.Empty;

            if ( !path.StartsWith("http://") && !path.StartsWith("https://") )
            {
                path = "http://" + path;
            }

            try
            {
                using ( HttpResponseMessage response = await httpClient.GetHeadersAsync(path) )
                {
                    if ( response.StatusCode != HttpStatusCode.OK )
                    {
                        return new HttpStatusCodeResult(response.StatusCode);
                    }
                    contentType = response.Content.Headers.ContentType.MediaType;
                }
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImageProcessor processor = new ImageProcessor();

            MemoryStream contentStream = await processor.RunAsync(path);

            return File(contentStream, contentType);
        }
    }
}