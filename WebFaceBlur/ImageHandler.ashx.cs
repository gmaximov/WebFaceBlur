using Microsoft.ProjectOxford.Face.Contract;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Web;

namespace WebFaceBlur
{
    /// <summary>
    /// Сводное описание для ImageHandler
    /// </summary>
    public class ImageHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if ( string.IsNullOrEmpty(context.Request["src"]) )
            {
                return;
            }

            var request = (HttpWebRequest) WebRequest.Create(context.Request["src"]);

            using ( var response = (HttpWebResponse) request.GetResponse() )
            {
                using ( var responseStream = response.GetResponseStream() )
                {
                    var mime = response.ContentType;
                    var stream = new MemoryStream();

                    responseStream.CopyTo(stream);

                    if ( mime.ToLower().Contains("image") )
                    {
                        Rectangle[] faceRects = FaceDetection.Detect(context.Request["src"]);

                        
                        if ( faceRects.Length > 0 )
                        {
                            stream.Position = 0;
                            Bitmap bitmap = new Bitmap(stream);

                            bitmap = ImageProcessor.Blur(bitmap, faceRects);

                            stream.Position = 0;
                            bitmap.Save(stream, ImageFormat.Jpeg);
                        }
                    }
                    stream.Position = 0;
                    context.Response.StatusCode = (int) HttpStatusCode.OK;
                    context.Response.ContentType = mime;
                    context.Response.BinaryWrite(stream.ToArray());
                }
            }
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}