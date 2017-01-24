using System.Drawing;
using System.Drawing.Drawing2D;

namespace WebFaceBlur
{
    public class CircularBlur : IImageEffect
    {
        private int blurStrength;
        private IBlurAlgorithm blurAlgorithm;

        public CircularBlur(IBlurAlgorithm blurAlgorithm, int blurStrength = 10)
        {
            this.blurAlgorithm = blurAlgorithm;
            this.blurStrength = blurStrength;
        }

        public Bitmap Apply(Bitmap image, Rectangle[] rectangles)
        {
            Bitmap mainclone = new Bitmap(image);
            using ( Graphics g = Graphics.FromImage(mainclone) )
            {
                foreach ( var rect in rectangles )
                {
                    Bitmap cloneBitmap = image.Clone(rect, image.PixelFormat);

                    cloneBitmap = blurAlgorithm.Run(cloneBitmap, blurStrength);

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