using System.Drawing;
using System.Drawing.Drawing2D;

namespace WebFaceBlur.App.ImageEffect.Blur
{
    public class BlurEffect : IImageEffect
    {
        protected internal IBlurAlgorithm blurAlgorithm;

        public BlurEffect() : this(ServiceLocator.Resolve<IBlurAlgorithm>())
        {
        }

        public BlurEffect(IBlurAlgorithm blurAlgorithm)
        {
            this.blurAlgorithm = blurAlgorithm;
        }

        public Bitmap Apply(Bitmap image, Rectangle[] rectangles)
        {
            Bitmap mainclone = new Bitmap(image);
            using ( Graphics g = Graphics.FromImage(mainclone) )
            {
                foreach ( var rect in rectangles )
                {
                    Bitmap cloneBitmap = image.Clone(rect, image.PixelFormat);

                    cloneBitmap = blurAlgorithm.Run(cloneBitmap);

                    GraphicsPath path = new GraphicsPath();
                    
                    path.AddEllipse(rect);
                    g.Clip = new Region(path);

                    g.DrawImage(cloneBitmap, rect.Left, rect.Top);
                }
            }
            return mainclone;
        }
    }
}