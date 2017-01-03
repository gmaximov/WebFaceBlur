using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Web;

namespace WebFaceBlur
{
    public class ImageCacheInMemory : IImageCache
    {
        private static readonly TimeSpan cacheCleanThreshold = TimeSpan.FromHours(5);
        private static readonly  TimeSpan cacheCleanPeriod = TimeSpan.FromHours(1);
        

        private Timer cacheCleaner;

        private Dictionary<string, DetectedImage> dict = new Dictionary<string, DetectedImage>();

        public ImageCacheInMemory() : this(cacheCleanThreshold, cacheCleanPeriod)
        {
        }
        public ImageCacheInMemory(TimeSpan cacheCleanThreshold, TimeSpan cacheCleanPeriod)
        {
            cacheCleaner = new Timer(CleanCache, null, TimeSpan.Zero, cacheCleanPeriod);
        }

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
                    return;
                }
                dict.Add(fileName, new DetectedImage(checksum, faceRects, DateTime.Now + this.cacheCleanThreshold));
            }
        }

        private void CleanCache(object stateInfo)
        {
            lock ( dict )
            {
                List<string> toRemove = dict.Where(e => (e.Value.expireTime < DateTime.Now)).Select(e => e.Key).ToList();

                foreach(string key in toRemove )
                {
                    dict.Remove(key);
                }
            }
        }
    }
}