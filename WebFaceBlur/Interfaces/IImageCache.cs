using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace WebFaceBlur
{
    public interface IImageCache
    {
        Rectangle[] Get(string fileName, string checksum);
        void Put(string fileName, string checksum, Rectangle[] faceRects);
    }
}