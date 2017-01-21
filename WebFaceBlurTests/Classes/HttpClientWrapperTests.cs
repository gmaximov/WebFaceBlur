using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFaceBlur.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFaceBlur.Classes.Tests
{
    [TestClass()]
    public class HttpClientWrapperTests
    {
        [TestMethod()]
        public async void GetContentType_Image_Test()
        {
            HttpClientWrapper httpclient = new HttpClientWrapper();
            Uri uri = new Uri("http://www.iconsdb.com/icons/preview/white/square-xxl.png");

            var headers = await httpclient.GetHeadersAsync(uri);

            string contentType = headers.ContentType.MediaType;

            Assert.IsTrue(contentType.Contains("image"));
        }

        [TestMethod()]
        public async void GetContentType_ImagePNG_Test()
        {
            HttpClientWrapper httpclient = new HttpClientWrapper();
            Uri uri = new Uri("http://www.iconsdb.com/icons/preview/white/square-xxl.png");

            var headers = await httpclient.GetHeadersAsync(uri);

            string contentType = headers.ContentType.MediaType;

            Assert.IsTrue(contentType.Contains("image/png"));
        }

        [TestMethod()]
        public async void GetContentType_Html_Test()
        {
            HttpClientWrapper httpclient = new HttpClientWrapper();
            Uri uri = new Uri("http://www.iconsdb.com/");

            var headers = await httpclient.GetHeadersAsync(uri);

            string contentType = headers.ContentType.MediaType;

            Assert.IsTrue(contentType.Contains("html"));
        }
    }
}