using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
                Uri.IsWellFormedUriString(path, UriKind.Absolute);
                if ( !Uri.TryCreate(path, UriKind.Absolute, out uri) && (!Uri.TryCreate("http://" + path, UriKind.Absolute, out uri)) )
                {
                    throw new Exception();
                }
                
                MemoryStream contentStream = null;
                string contentType = string.Empty;
                string contentHtml = string.Empty;

                try
                {
                    using ( var response = await HttpClientWrapper.GetAsync(uri) )
                    {
                        if ( !response.IsSuccessStatusCode )
                        {
                            throw new Exception();
                        }

                        contentType = response.Content.Headers.ContentType.MediaType;

                        if( contentType.Contains("html") )
                        {
                            contentHtml = await response.Content.ReadAsStringAsync();
                        }
                        else
                        {
                            using ( Stream stream = await response.Content.ReadAsStreamAsync() )
                            {
                                contentStream = new MemoryStream();
                                await stream.CopyToAsync(contentStream);
                                contentStream.Position = 0;
                            }
                        }

                    }
                }
                catch
                {
                    //write base64 part
                    throw;
                }

                if(contentType == string.Empty )
                {
                    throw new Exception();
                }


                if ( contentType.Contains("html") )
                {
                    contentHtml = await Processor.Html(contentHtml, uri);
                    ViewBag.Content = contentHtml;
                    return View("Index");
                }
                else
                {
                    if ( contentType.Contains("image") )
                    {
                        contentStream = await Processor.ImageFile(contentStream, uri) as MemoryStream;
                    }
                    else
                    {
                        contentStream = await Processor.Default(contentStream, uri) as MemoryStream;
                    }

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