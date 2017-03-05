using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebFaceBlur.App.FaceDetection
{
    class MicrosoftFaceDetection : IFaceDetection
    {
        protected internal IFaceServiceClient faceServiceClient;

        public MicrosoftFaceDetection()
        {
            if (Config.MicrosoftFaceSubscriptionKey == string.Empty )
            {
                throw new Exception("Microsoft Face subscription key is empty.");
            }
            faceServiceClient = new FaceServiceClient(Config.MicrosoftFaceSubscriptionKey);
        }

        public async Task<Rectangle[]> Detect(string path)
        {
            try
            {
                var faces = await faceServiceClient.DetectAsync(path.ToString());
                var faceRects = faces.Select(face => new Rectangle(face.FaceRectangle.Left, face.FaceRectangle.Top, face.FaceRectangle.Width-1, face.FaceRectangle.Height-1) );
                return faceRects.ToArray();
            }
            catch ( Exception )
            {
                return null;
            }
        }
    }
}
