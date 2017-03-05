using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFaceBlur
{
    public interface IFaceDetection
    {
        Task<Rectangle[]> Detect(string path);
    }
}
