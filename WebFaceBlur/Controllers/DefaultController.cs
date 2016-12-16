using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebFaceBlur.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index(string path = "https%3A%2F%2Fwww.google.ru%2Fsearch%3Fq%3Dfaces%26newwindow%3D1%26tbm%3Disch" )
        {
            try
            {
                ViewData["url_error"] = false;

                Uri uri;
                path = Server.UrlDecode(path);
                if ( !Uri.TryCreate(path, UriKind.Absolute, out uri) )
                {
                    ViewData["url_error"] = true;
                    return View();
                }

                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = web.Load(path);

                var nodes = doc.DocumentNode.SelectNodes("//img/@src");
                foreach ( HtmlNode node in nodes )
                {
                    if ( Uri.IsWellFormedUriString(node.Attributes["src"].Value, UriKind.RelativeOrAbsolute) )
                    {
                        Uri newUri = null;

                        if ( Uri.IsWellFormedUriString(node.Attributes["src"].Value, UriKind.Relative) )
                        {
                            Uri.TryCreate(uri, node.Attributes["src"].Value, out newUri);
                        }

                        if ( newUri != null )
                        {
                            node.SetAttributeValue("src", "ImageHandler.ashx?src=" + newUri.ToString());
                        }
                        else
                        {
                            node.SetAttributeValue("src", "ImageHandler.ashx?src=" + node.Attributes["src"].Value);
                        }
                    }
                }
                ViewData["page"] = doc.DocumentNode.OuterHtml;
                return View();
            }
            catch
            {

            }
            return View();
        }
        [HttpPost]
        public ActionResult Path(string path)
        {
            path = Server.UrlEncode(path);
            return Redirect("/?path=" + path);
        }
    }
}