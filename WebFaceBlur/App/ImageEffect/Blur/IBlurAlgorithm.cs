using System.Drawing;

namespace WebFaceBlur.App.ImageEffect.Blur
{
    public interface IBlurAlgorithm
    {
        Bitmap Run(Bitmap image);
    }
}
