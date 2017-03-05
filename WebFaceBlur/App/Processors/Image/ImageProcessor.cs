using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebFaceBlur.App.Http;
using WebFaceBlur.App.ImageCache;
using WebFaceBlur.App.ImageEffect;

namespace WebFaceBlur.App.Processors.Image
{
    public class ImageProcessor : IImageProcessorAsync
    {
        protected internal IHttpClientWrapperAsync httpClient;
        protected internal IFaceDetection faceDetection;
        protected internal IImageCache imageCache;
        protected internal IImageEffect imageEffect;

        public ImageProcessor() : this(ServiceLocator.Resolve<IHttpClientWrapperAsync>(), ServiceLocator.Resolve<IFaceDetection>(), ServiceLocator.Resolve<IImageCache>(), ServiceLocator.Resolve<IImageEffect>())
        {
        }

        public ImageProcessor(IHttpClientWrapperAsync httpClient, IFaceDetection faceDetection, IImageCache imageCache, IImageEffect imageEffect)
        {
            this.httpClient = httpClient;
            this.faceDetection = faceDetection;
            this.imageCache = imageCache;
            this.imageEffect = imageEffect;
        }

        public async Task<MemoryStream> RunAsync(string path)
        {
            MemoryStream memoryStream = new MemoryStream();
            using ( Stream stream = await httpClient.GetStreamAsync(path) )
            {
                await stream.CopyToAsync(memoryStream);
            }
            memoryStream.Position = 0;
            string checksum = ImageCacheUtils.GetChecksum(memoryStream);

            Rectangle[] faceRects = imageCache.Get(path.ToString(), checksum);

            if ( faceRects == null )
            {
                faceRects = await faceDetection.Detect(path);

                if ( faceRects == null )
                {
                    memoryStream.Position = 0;
                    return memoryStream;
                }

                imageCache.Add(path.ToString(), checksum, faceRects);
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