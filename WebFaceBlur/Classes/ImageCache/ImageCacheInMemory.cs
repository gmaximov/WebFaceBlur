using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Web;

namespace WebFaceBlur
{
    public class ImageCacheInMemory : IImageCache
    {
        private static TimeSpan expireTime = TimeSpan.FromHours(1);
        private static readonly object cacheLock = new object();

        public ImageCacheInMemory() : this(expireTime)
        {
        }
        public ImageCacheInMemory(TimeSpan expireTime)
        {
            ImageCacheInMemory.expireTime = expireTime;
        }

        public Rectangle[] Get(string fileName, string checksum)
        {
            lock ( cacheLock )
            {
                MemoryCache memoryCache = MemoryCache.Default;
                if( !memoryCache.Contains(fileName) )
                {
                    return null;
                }

                DetectedImage image = (memoryCache.Get(fileName) as DetectedImage);
                if( image.checksum != checksum )
                {
                    return null;
                }

                return image.faceRects;
            }           
        }

        public void Add(string fileName, string checksum, Rectangle[] faceRects)
        {
            lock( cacheLock )
            {
                MemoryCache memoryCache = MemoryCache.Default;
                if ( memoryCache.Contains(fileName) )
                {
                    bool sameChecksum = (checksum == (memoryCache.Get(fileName) as DetectedImage).checksum);
                    if ( sameChecksum )
                    {
                        return;
                    }
                    memoryCache.Remove(fileName);
                }
                memoryCache.Add(fileName, new DetectedImage(checksum, faceRects), DateTime.Now.Add(expireTime));
            }
        }
    }

}