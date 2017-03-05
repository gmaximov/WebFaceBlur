using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace WebFaceBlur.App.ImageCache
{
    public class DetectedImage
    {
        public string checksum;
        public Rectangle[] faceRects;

        public DetectedImage(string checksum, Rectangle[] faceRects)
        {
            this.checksum = checksum;
            this.faceRects = faceRects;
        }
    }
}