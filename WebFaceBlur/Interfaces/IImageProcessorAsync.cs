using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFaceBlur
{
    public interface IImageProcessorAsync
    {
        Task<MemoryStream> RunAsync();
    }
}
