using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace _9Converter
{
    public class NinePatchResizer:IResizer
    {
        public Bitmap[] ResizeImage(string filename)
        {
            Bitmap Source = SourceImageFactory.NonLockingOpen(filename);
            int sourceWidth = Source.Width;
            int sourceHeight = Source.Height;

            PatchFrame SourceFrame = new PatchFrame();
            SourceFrame.Fill(Source);

            PatchFrame ResFrame6 = SourceFrame.Resize(6);
            PatchFrame ResFrame4 = SourceFrame.Resize(4);
            PatchFrame ResFrame3 = SourceFrame.Resize(3);

            #region Crop, Resize and Expand Source Image
            ImageProcessor imgProc = new ImageProcessor();

            //Crop
            Bitmap cropSource = imgProc.Crop(Source);

            //Resize
            Bitmap resizingImage6 = imgProc.Resize(cropSource, sourceWidth * 6 / 8, sourceHeight * 6 / 8);
            Bitmap resizingImage4 = imgProc.Resize(cropSource, sourceWidth / 2, sourceHeight / 2);
            Bitmap resizingImage3 = imgProc.Resize(cropSource, sourceWidth * 3 / 8, sourceHeight * 3 / 8,
                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic);

            //Expand
            Bitmap expandedImage6 = imgProc.Expand(resizingImage6);
            Bitmap expandedImage4 = imgProc.Expand(resizingImage4);
            Bitmap expandedImage3 = imgProc.Expand(resizingImage3);
            #endregion

            #region Draw Nine_Stamp
            imgProc.Draw9PatchStamp(ResFrame6, expandedImage6);
            imgProc.Draw9PatchStamp(ResFrame4, expandedImage4);
            imgProc.Draw9PatchStamp(ResFrame3, expandedImage3);
            #endregion

            Bitmap[] result = new Bitmap[]{Source, expandedImage6, expandedImage4, expandedImage3};
            return result;
        }
    }
}
