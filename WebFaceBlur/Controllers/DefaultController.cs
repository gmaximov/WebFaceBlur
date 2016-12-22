using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
                path = Server.UrlDecode(path);

                Uri uri = null;
                if ( !Uri.TryCreate(path, UriKind.Absolute, out uri) && (!Uri.TryCreate("http://" + path, UriKind.Absolute, out uri)) )
                {
                    throw new Exception();
                }

                string content = GetContent(uri);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(content);
                doc = FixLinks(doc, uri);
                

                ViewData["content"] = doc.DocumentNode.OuterHtml;
                return View();
            }
            catch
            {
                ViewData["isError"] = true;
                return View();
            }
        }
        [HttpPost]
        public ActionResult Path(string path)
        {
            path = Server.UrlEncode(Server.UrlEncode(path));
            return Redirect("/?path=" + path);
        }

        private string GetContent(Uri uri)
        {
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(uri);

                using ( var response = (HttpWebResponse) request.GetResponse() )
                {
                    var mime = response.ContentType;

                    if ( mime.ToLower().Contains("image") )
                    {
                        return "<img src=" + uri.ToString() + ">";
                    }
                    else if ( mime.ToLower().Contains("text") )
                    {
                        var encoding = Encoding.GetEncoding(response.CharacterSet);
                        using ( var responseStream = response.GetResponseStream() )
                        {

                            using ( var reader = new StreamReader(responseStream, encoding) )
                            {
                                return reader.ReadToEnd();
                            }
                        }
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private HtmlDocument FixLinks(HtmlDocument doc, Uri uri)
        {
            doc = Fix_IMG_Tag(doc, uri);
            doc = Fix_A_Tag(doc, uri);
            doc = Fix_LINK_Tag(doc, uri);
            doc = Fix_SCRIPT_Tag(doc, uri);
            return doc;
        }

        private HtmlDocument Fix_IMG_Tag(HtmlDocument doc, Uri uri)
        {
            var nodes = doc.DocumentNode.SelectNodes("//img/@src");
            if ( nodes != null )
            {
                foreach ( HtmlNode node in nodes )
                {
                    if ( Uri.IsWellFormedUriString(node.Attributes["src"].Value, UriKind.RelativeOrAbsolute) )
                    {
                        Uri newUri = null;

                        if ( Uri.IsWellFormedUriString(node.Attributes["src"].Value, UriKind.Relative) )
                        {
                            newUri = new Uri(uri, node.Attributes["src"].Value);
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
            }
            return doc;
        }
        private HtmlDocument Fix_SCRIPT_Tag(HtmlDocument doc, Uri uri)
        {
            var nodes = doc.DocumentNode.SelectNodes("//script/@src");
            if ( nodes != null )
            {
                foreach ( HtmlNode node in nodes )
                {
                    if ( Uri.IsWellFormedUriString(node.Attributes["src"].Value, UriKind.RelativeOrAbsolute) )
                    {
                        Uri newUri = null;

                        if ( Uri.IsWellFormedUriString(node.Attributes["src"].Value, UriKind.Relative) )
                        {
                            newUri = new Uri(uri, node.Attributes["src"].Value);
                        }

                        if ( newUri != null )
                        {
                            node.SetAttributeValue("src", newUri.ToString());
                        }
                    }
                }
            }
            return doc;
        }
        private HtmlDocument Fix_A_Tag(HtmlDocument doc, Uri uri)
        {
            var nodes = doc.DocumentNode.SelectNodes("//a/@href");
            if ( nodes != null )
            {
                foreach ( HtmlNode node in nodes )
                {
                    if ( Uri.IsWellFormedUriString(node.Attributes["href"].Value, UriKind.RelativeOrAbsolute) )
                    {
                        Uri newUri = null;

                        if ( Uri.IsWellFormedUriString(node.Attributes["href"].Value, UriKind.Relative) )
                        {
                            newUri = new Uri(uri, node.Attributes["href"].Value);
                        }

                        if ( newUri != null )
                        {
                            node.SetAttributeValue("href", "?path=" + Server.UrlEncode(newUri.ToString()));
                        }
                        else
                        {
                            node.SetAttributeValue("href", "?path=" + Server.UrlEncode(node.Attributes["href"].Value));
                        }
                    }
                }
            }
            return doc;
        }
        private HtmlDocument Fix_LINK_Tag(HtmlDocument doc, Uri uri)
        {
            var nodes = doc.DocumentNode.SelectNodes("//link/@href");
            if ( nodes != null )
            {
                foreach ( HtmlNode node in nodes )
                {
                    if ( Uri.IsWellFormedUriString(node.Attributes["href"].Value, UriKind.RelativeOrAbsolute) )
                    {
                        Uri newUri = null;

                        if ( Uri.IsWellFormedUriString(node.Attributes["href"].Value, UriKind.Relative) )
                        {
                            newUri = new Uri(uri, node.Attributes["href"].Value);
                        }

                        if ( newUri != null )
                        {
                            node.SetAttributeValue("href", newUri.ToString());
                        }
                    }
                }
            }
            return doc;
        }
    }
}