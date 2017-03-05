using WebFaceBlur.App;
using WebFaceBlur.App.FaceDetection;
using WebFaceBlur.App.Http;
using WebFaceBlur.App.ImageCache;
using WebFaceBlur.App.ImageEffect;
using WebFaceBlur.App.ImageEffect.Blur;

namespace WebFaceBlur
{
    public class ServiceLocatorConfig
    {
        public static void RegisterServices()
        {
            ServiceLocator.RegisterService<IImageCache>(typeof(ImageCacheInMemory));
            ServiceLocator.RegisterService<IImageEffect>(typeof(BlurEffect));
            ServiceLocator.RegisterService<IHttpClientWrapperAsync>(typeof(HttpClientWrapper));
            ServiceLocator.RegisterService<IFaceDetection>(typeof(MicrosoftFaceDetection));
            ServiceLocator.RegisterService<IBlurAlgorithm>(typeof(FastGaussianBlur));
        }
    }
}