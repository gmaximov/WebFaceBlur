using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFaceBlur.App
{
    public class Config
    {
        public static string MicrosoftFaceSubscriptionKey = "92fe12c3a08b48578f26cdf030d613f0";
        public static string CDNAdress = "https://webfaceblur-cdn.azureedge.net";
        public static TimeSpan CacheLifeTime = TimeSpan.FromHours(1);
        public static int BlurStrength = 15;
    }
}