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
        public void GetContentType_Image_Test()
        {
            string str = HttpClientWrapper.GetContentType("http://www.iconsdb.com/icons/preview/white/square-xxl.png").Result;          
            Assert.IsTrue(str.Contains("image"));
        }

        [TestMethod()]
        public void GetContentType_ImagePNG_Test()
        {
            string str = HttpClientWrapper.GetContentType("http://www.iconsdb.com/icons/preview/white/square-xxl.png").Result;
            Assert.IsTrue(str.Contains("image/png"));
        }

        [TestMethod()]
        public void GetContentType_Html_Test()
        {
            string str = HttpClientWrapper.GetContentType("http://www.iconsdb.com/").Result;
            Assert.IsTrue(str.Contains("html"));
        }
    }
}