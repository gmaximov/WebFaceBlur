﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFaceBlur.App.ImageEffect
{
    public interface IImageEffect
    {
        Bitmap Apply(Bitmap image, Rectangle[] rectangles);
    }
}
