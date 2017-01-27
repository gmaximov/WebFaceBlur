using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFaceBlur
{
    public class Config
    {
        public static string MicrosoftFaceSubscriptionKey = "";
        public static string CDNAdress = "";
        public static TimeSpan CacheLifeTime = TimeSpan.FromHours(1);
    }
}
