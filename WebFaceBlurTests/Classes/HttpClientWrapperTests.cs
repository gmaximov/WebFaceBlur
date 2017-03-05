using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFaceBlur.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFaceBlur.App.Http;

namespace WebFaceBlur.Classes.Tests
{
    [TestClass()]
    public class HttpClientWrapperTests
    {
        [TestMethod()]
        public async void GetContentType_Image_Test()
        {
            HttpClientWrapper httpclient = new HttpClientWrapper();
            string path = "http://www.iconsdb.com/icons/preview/white/square-xxl.png";

            var response = await httpclient.GetHeadersAsync(path);

            string contentType = response.Content.Headers.ContentType.MediaType;

            Assert.IsTrue(contentType.Contains("image"));
        }

        [TestMethod()]
        public async void GetContentType_ImagePNG_Test()
        {
            HttpClientWrapper httpclient = new HttpClientWrapper();
            string path = "http://www.iconsdb.com/icons/preview/white/square-xxl.png";

            var response = await httpclient.GetHeadersAsync(path);

            string contentType = response.Content.Headers.ContentType.MediaType;

            Assert.IsTrue(contentType.Contains("image/png"));
        }

        [TestMethod()]
        public async void GetContentType_Html_Test()
        {
            HttpClientWrapper httpclient = new HttpClientWrapper();
            string path = "http://www.iconsdb.com/";

            var response = await httpclient.GetHeadersAsync(path);

            string contentType = response.Content.Headers.ContentType.MediaType;

            Assert.IsTrue(contentType.Contains("html"));
        }
    }
}