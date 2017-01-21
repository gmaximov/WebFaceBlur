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
        private Uri uri;

        public HtmlProcessor(IHttpClientWrapperAsync httpClient, Uri uri)
        {
            this.httpClient = httpClient;
            this.uri = uri;
        }

        public async Task<string> RunAsync()
        {
            string htmlContent = await httpClient.GetStringAsync(uri);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            FixTag(ref doc, uri, "img", "src", true, "?path=");
            FixTag(ref doc, uri, "a", "href", true, "?path=");
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