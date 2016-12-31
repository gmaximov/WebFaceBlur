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
    public class HtmlContentProcessor : IHtmlContentProcessor
    {
        public async Task<string> Process(string html, Uri uri)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            doc = FixLinks(doc, uri);
            return doc.DocumentNode.OuterHtml;
        }
        private HtmlDocument FixLinks(HtmlDocument doc, Uri uri)
        {
            FixTag(ref doc, uri, "img", "src", true, "?path=");
            FixTag(ref doc, uri, "a", "href", true, "?path=");
            FixTag(ref doc, uri, "link", "href");
            FixTag(ref doc, uri, "script", "src");

            //FixTag(ref doc, uri, "form", "action");

            return doc;
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
                        Uri newUri = null;
                        string attributeValue = node.Attributes[attribute].Value;

                        if ( encode )
                        {
                            attributeValue = HttpUtility.HtmlDecode(attributeValue);
                        }

                        if ( Uri.IsWellFormedUriString(node.Attributes[attribute].Value, UriKind.Relative) )
                        {
                            newUri = new Uri(uri, attributeValue);
                        }

                        if (newUri != null )
                        {

                            attributeValue = newUri.ToString();
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