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


            var httpWebRequest = (HttpWebRequest) WebRequest.Create(context.Request["src"]);
            var httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();
            // Get the stream associated with the response.
            var receiveStream = httpWebResponse.GetResponseStream();
            var mime = httpWebResponse.ContentType;

            var stream = new MemoryStream();
            receiveStream.CopyTo(stream);

            FaceRectangle[] faceRects = FaceDetection.Detect(context.Request["src"]);

            if ( faceRects.Length > 0 )
            {
                stream.Position = 0;
                Bitmap bitmap = new Bitmap(stream);
                foreach ( var faceRect in faceRects )
                {

                    bitmap = ImageProcessor.Blur(bitmap,
                        new Rectangle(faceRect.Left, faceRect.Top, faceRect.Width, faceRect.Height));
                }
                stream.Position = 0;
                bitmap.Save(stream, ImageFormat.Jpeg);
            }

            stream.Position = 0;
            context.Response.StatusCode = (int) HttpStatusCode.OK;
            context.Response.ContentType = mime;
            context.Response.BinaryWrite(stream.ToArray());

            httpWebResponse.Close();
            try
            {}
            catch
            {}
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