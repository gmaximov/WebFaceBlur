using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace WebFaceBlur
{
    public class ImageCacheStub : IImageCache
    {
        public void Add(string fileName, string checksum, Rectangle[] faceRects)
        {
            
        }

        public Rectangle[] Get(string fileName, string checksum)
        {
            return null;
        }
    }
}