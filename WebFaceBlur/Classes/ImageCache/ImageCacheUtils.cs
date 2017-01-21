using System;
using System.IO;
using System.Security.Cryptography;

namespace WebFaceBlur
{
    public class ImageCacheUtils
    {
        public static string GetChecksum(Stream stream)
        {
            SHA256Managed sha = new SHA256Managed();
            byte[] checksum = sha.ComputeHash(stream);
            return BitConverter.ToString(checksum).Replace("-", String.Empty);
        }
    }
}