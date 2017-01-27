using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace WebFaceBlur.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        [OutputCache(VaryByParam = "*", Duration = 1, Location = OutputCacheLocation.Downstream)]
        public async Task<ActionResult> Index(string path)
        {
            if (path == null)
            {
                return View();
            }

            path = HttpUtility.UrlDecode(path);

            HttpClientWrapper httpClient = new HttpClientWrapper();

            string contentType = string.Empty;

            if(!path.StartsWith("http://") && !path.StartsWith("https://"))
            {
                path = "http://" + path;
            }

            try
            {
                using ( HttpResponseMessage response = await httpClient.GetHeadersAsync(path) )
                {
                    if (response.StatusCode != HttpStatusCode.OK)
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
            

            if ( contentType.Contains("html") )
            {
                HtmlProcessor processor = new HtmlProcessor(httpClient, "/?path=", Config.CDNAdress);

                string contentHtml = await processor.RunAsync(path);

                ViewBag.Content = contentHtml;
                return View("Index");
            }
            else
            {
                MemoryStream contentStream;

                if ( contentType.Contains("image") )
                {
                    return Redirect(string.Format("/Image/?path={0}", path));
                }
                else
                {
                    DefaultProcessor processor = new DefaultProcessor(httpClient);

                    contentStream = await processor.RunAsync(path);

                    contentStream.Position = 0;
                    return File(contentStream, contentType);
                }
            }
        }
    }
}