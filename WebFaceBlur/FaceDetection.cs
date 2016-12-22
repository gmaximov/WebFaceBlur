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
    class FaceDetection
    {
        private static string key = "";
        private static IFaceServiceClient faceServiceClient = new FaceServiceClient(key);

        public static Rectangle[] Detect(string url)
        {
            if ( key == "" )
            {
                throw new Exception("Configure subscription key in WebFaceBlur.FaceDetection");
            }
            try
            {
                var faces = AsyncHelpers.RunSync<Face[]>(() => faceServiceClient.DetectAsync(url));
                var faceRects = faces.Select(face => new Rectangle(face.FaceRectangle.Left, face.FaceRectangle.Top, face.FaceRectangle.Width, face.FaceRectangle.Height) );
                return faceRects.ToArray();
            }
            catch ( Exception )
            {
                return new Rectangle[0];
            }
        }
    }
}
