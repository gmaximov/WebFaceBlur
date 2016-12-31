using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace WebFaceBlur
{
    public class ImageCache
    {
        private static IImageCache cache = new ImageCacheInMemory();

        public static Rectangle[] Get(string fileName, string checksum)
        {
            return cache.Get(fileName, checksum);
        }

        public static void Put(string fileName, string checksum, Rectangle[] faceRects)
        {
            cache.Put(fileName, checksum, faceRects);
        }

        public static string GetChecksum(Stream stream)
        {
            SHA256Managed sha = new SHA256Managed();
            byte[] checksum = sha.ComputeHash(stream);
            return BitConverter.ToString(checksum).Replace("-", String.Empty);
        }
    }

}