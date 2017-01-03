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

namespace WebFaceBlur.Tests
{
    [TestClass()]
    public class ImageCacheInMemoryTests
    {
        [TestMethod()]
        public void PutAndGetTest()
        {
            ImageCacheInMemory cache = new ImageCacheInMemory();

            Assert.IsNull(cache.Get("test", "test"));
            Assert.IsNull(cache.Get("test1", "test1"));

            cache.Put("test", "test", new Rectangle[2]);

            Assert.IsNotNull(cache.Get("test", "test"));
            Assert.IsNull(cache.Get("test1", "test1"));
        }
        [TestMethod()]
        public void CleanerWithRemovingTest()
        {
            ImageCacheInMemory cache = new ImageCacheInMemory(TimeSpan.FromSeconds(4), TimeSpan.FromSeconds(5));

            Assert.IsNull(cache.Get("test", "test"));

            cache.Put("test", "test", new Rectangle[2]);

            Assert.IsNotNull(cache.Get("test", "test"));

            Task.Run(() => Thread.Sleep(10000)).Wait();

            Assert.IsNull(cache.Get("test", "test"));
        }
        [TestMethod()]
        public void CleanerWithoutRemovingTest()
        {
            ImageCacheInMemory cache = new ImageCacheInMemory(TimeSpan.FromSeconds(4), TimeSpan.FromSeconds(5));

            Assert.IsNull(cache.Get("test", "test"));

            cache.Put("test", "test", new Rectangle[2]);

            Assert.IsNotNull(cache.Get("test", "test"));

            Task.Run(() => Thread.Sleep(2000)).Wait();

            Assert.IsNotNull(cache.Get("test", "test"));
        }
    }
}