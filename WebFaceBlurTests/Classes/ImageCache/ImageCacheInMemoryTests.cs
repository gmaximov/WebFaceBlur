using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using WebFaceBlur;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Moq;
using WebFaceBlur.App.ImageCache;

namespace WebFaceBlur.Tests
{
    [TestClass()]
    public class ImageCacheInMemoryTests
    {
        [TestMethod()]
        public void PutAndGetTest()
        {
            ImageCacheInMemory cache = new ImageCacheInMemory(TimeSpan.FromSeconds(3));

            Assert.IsNull(cache.Get("test", "test"));
            Assert.IsNull(cache.Get("test1", "test1"));

            cache.Add("test", "test", new Rectangle[2]);

            Assert.IsNotNull(cache.Get("test", "test"));
            Assert.IsNull(cache.Get("test1", "test1"));

        }
        [TestMethod()]
        public void CleanerWithRemovingTest()
        {
            ImageCacheInMemory cache = new ImageCacheInMemory(TimeSpan.FromMilliseconds(200));

            Assert.IsNull(cache.Get("test3", "test3"));

            cache.Add("test3", "test3", new Rectangle[2]);

            Assert.IsNotNull(cache.Get("test3", "test3"));

            Task.Run(() => Thread.Sleep(300)).Wait();

            Assert.IsNull(cache.Get("test3", "test3"));
        }
        [TestMethod()]
        public void CleanerWithoutRemovingTest()
        {
            ImageCacheInMemory cache = new ImageCacheInMemory(TimeSpan.FromMilliseconds(200));

            Assert.IsNull(cache.Get("test4", "test4"));

            cache.Add("test4", "test4", new Rectangle[2]);

            Assert.IsNotNull(cache.Get("test4", "test4"));

            Task.Run(() => Thread.Sleep(50)).Wait();

            Assert.IsNotNull(cache.Get("test4", "test4"));
        }
    }
}