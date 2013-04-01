using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace _9Converter
{
    public class ImageResizer:IResizer
    {
        public Bitmap[] ResizeImage(string filename)
        {
            Bitmap Source = SourceImageFactory.NonLockingOpen(filename);
            int sourceWidth = Source.Width;
            int sourceHeight = Source.Height;

            #region Resize Source Image

            ImageProcessor imgProc = new ImageProcessor();
            //Resize
            Bitmap resizingImage6 = imgProc.Resize(Source, sourceWidth * 6 / 8, sourceHeight * 6 / 8);
            Bitmap resizingImage4 = imgProc.Resize(Source, sourceWidth / 2, sourceHeight / 2);
            Bitmap resizingImage3 = imgProc.Resize(Source, sourceWidth * 3 / 8, sourceHeight * 3 / 8,
                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic);
            #endregion

            Bitmap[] result = new Bitmap[] { Source, resizingImage6, resizingImage4, resizingImage3 };
            return result;
        }
    }
}
