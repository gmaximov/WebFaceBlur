using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebFaceBlur
{
    public class FaceDetection
    {
        private static IFaceDetection faceDetector = new MicrosoftFaceDetection();

        public async static Task<Rectangle[]> Detect(Uri uri)
        {
            return await faceDetector.Detect(uri);
        }
    }
}