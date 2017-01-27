using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebFaceBlur
{
    public class HtmlProcessor : IHtmlProcessorAsync
    {
        private IHttpClientWrapperAsync httpClient;
        private string defaultPath;
        private string CDNAdress;

        public HtmlProcessor(IHttpClientWrapperAsync httpClient, string defaultPath = "", string CDNAdress = "")
        {
            this.httpClient = httpClient;
            this.defaultPath = defaultPath;
            this.CDNAdress = CDNAdress;
        }

        public async Task<string> RunAsync(string path)
        {
            string htmlContent = await httpClient.GetStringAsync(path);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            Uri uri = new Uri(path);
            FixTag(ref doc, uri, "img", "src", true, CDNAdress + defaultPath);
            FixTag(ref doc, uri, "a", "href", true, defaultPath);
            FixTag(ref doc, uri, "link", "href");
            FixTag(ref doc, uri, "script", "src");

            return doc.DocumentNode.OuterHtml;
        }

        private void FixTag(ref HtmlDocument doc, Uri uri, string tag, string attribute, bool encode = false, string prependString = "")
        {
            var nodes = doc.DocumentNode.SelectNodes("//"+ tag +"/@" + attribute);
            if ( nodes != null )
            {
                foreach ( HtmlNode node in nodes )
                {
                    if ( Uri.IsWellFormedUriString(node.Attributes[attribute].Value, UriKind.RelativeOrAbsolute) )
                    {
                        if ( node.Attributes[attribute].Value.StartsWith("data:"))
                        {
                            continue;
                        }
                        Uri newpath = null;

                        string attributeValue = node.Attributes[attribute].Value;

                        if ( encode )
                        {
                            attributeValue = HttpUtility.HtmlDecode(attributeValue);
                        }

                        if ( Uri.IsWellFormedUriString(attributeValue, UriKind.Relative) )
                        {
                            newpath = new Uri(uri, attributeValue);
                        }

                        if (newpath != null )
                        {

                            attributeValue = newpath.ToString();
                        }

                        if ( encode )
                        {
                            attributeValue = HttpUtility.UrlEncode(attributeValue);
                        }

                        if ( prependString != "" )
                        {
                            attributeValue = prependString + attributeValue;
                        }

                        node.SetAttributeValue(attribute, attributeValue);
                    }
                }
            }
        }
    }
}