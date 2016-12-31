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
        private static string key = "5d4b63e1117d4055a64e4365fe94fb0c";
        private static IFaceServiceClient faceServiceClient = new FaceServiceClient(key);

        public async Task<Rectangle[]> Detect(Uri uri)
        {
            if ( key == "" )
            {
                throw new Exception("Configure subscription key in WebFaceBlur.FaceDetection");
            }
            try
            {
                var faces = await faceServiceClient.DetectAsync(uri.ToString());
                var faceRects = faces.Select(face => new Rectangle(face.FaceRectangle.Left, face.FaceRectangle.Top, face.FaceRectangle.Width, face.FaceRectangle.Height) );
                return faceRects.ToArray();
            }
            catch ( Exception )
            {
                return null;
            }
        }
    }
}
