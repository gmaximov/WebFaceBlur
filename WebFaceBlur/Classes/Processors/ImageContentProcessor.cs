using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebFaceBlur
{
    public class ImageFileContentProcessor : IContentProcessor
    {
        public async Task<Stream> Process(Stream stream, Uri uri)
        {
            stream.Position = 0;
            string checksum = ImageCache.GetChecksum(stream);

            Rectangle[] faceRects = ImageCache.Get(uri.ToString(), checksum);

            if ( faceRects == null )
            {
                faceRects = await FaceDetection.Detect(uri);

                if (faceRects == null )
                {
                    stream.Position = 0;
                    return stream;
                }

                ImageCache.Put(uri.ToString(), checksum, faceRects);
            }

            if ( faceRects.Length > 0 )
            {
                stream.Position = 0;
                Bitmap bitmap = new Bitmap(stream);

                bitmap = Blur.Process(bitmap, faceRects);

                stream.Position = 0;
                bitmap.Save(stream, ImageFormat.Png);
            }
            stream.Position = 0;
            return stream;
        }
    }
}