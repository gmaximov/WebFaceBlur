using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace WebFaceBlur
{
    public class ImageCacheInMemory : IImageCache
    {
        public Dictionary<string, DetectedImage> dict = new Dictionary<string, DetectedImage>();

        public Rectangle[] Get(string fileName, string checksum)
        {
            lock ( dict )
            {
                if ( !dict.ContainsKey(fileName) )
                {
                    return null;
                }

                if ( dict[fileName].checksum != checksum )
                {
                    return null;
                }

                return dict[fileName].faceRects;
            }
        }

        public void Put(string fileName, string checksum, Rectangle[] faceRects)
        {
            lock ( dict )
            {
                if ( dict.ContainsKey(fileName) )
                {
                    dict.Remove(fileName);
                }
                dict.Add(fileName, new DetectedImage(checksum, faceRects));
            }
        }
    }
}