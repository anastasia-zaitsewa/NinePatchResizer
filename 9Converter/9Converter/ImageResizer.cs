/*
Copyright anastasia.zaitsewa@gmail.com Anastasia Zaitseva

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

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
