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
    public class ImageProcessor : IImageProcessorAsync
    {
        private IHttpClientWrapperAsync httpClient;
        private IFaceDetection faceDetection;
        private IImageCache imageCache;
        private IImageEffect imageEffect;

        public ImageProcessor(IHttpClientWrapperAsync httpClient, IFaceDetection faceDetection, IImageCache imageCache, IImageEffect imageEffect)
        {
            this.httpClient = httpClient;
            this.faceDetection = faceDetection;
            this.imageCache = imageCache;
            this.imageEffect = imageEffect;
        }

        public async Task<MemoryStream> RunAsync(Uri uri)
        {
            MemoryStream memoryStream = new MemoryStream();
            using ( Stream stream = await httpClient.GetStreamAsync(uri) )
            {
                await stream.CopyToAsync(memoryStream);
            }
            memoryStream.Position = 0;
            string checksum = ImageCacheUtils.GetChecksum(memoryStream);

            Rectangle[] faceRects = imageCache.Get(uri.ToString(), checksum);

            if ( faceRects == null )
            {
                faceRects = await faceDetection.Detect(uri);

                if ( faceRects == null )
                {
                    memoryStream.Position = 0;
                    return memoryStream;
                }

                imageCache.Add(uri.ToString(), checksum, faceRects);
            }

            if ( faceRects.Length > 0 )
            {
                memoryStream.Position = 0;
                Bitmap bitmap = new Bitmap(memoryStream);

                bitmap = imageEffect.Apply(bitmap, faceRects);

                memoryStream.Position = 0;
                bitmap.Save(memoryStream, ImageFormat.Png);
            }
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}