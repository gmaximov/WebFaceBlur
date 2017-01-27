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

namespace WebFaceBlur
{
    class MicrosoftFaceDetection : IFaceDetection
    {
        private IFaceServiceClient faceServiceClient;

        public MicrosoftFaceDetection(string key)
        {
            if (key == string.Empty )
            {
                throw new Exception("Microsoft Face subscription key is empty.");
            }
            faceServiceClient = new FaceServiceClient(key);
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
