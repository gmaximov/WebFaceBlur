using System.Drawing;

namespace WebFaceBlur
{
    public interface IBlurAlgorithm
    {
        Bitmap Run(Bitmap image, int strength);
    }
}
