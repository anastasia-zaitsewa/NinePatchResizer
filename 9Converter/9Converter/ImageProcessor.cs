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

using System.Collections.Generic;
using System.Drawing;

namespace _9Converter
{
    public class ImageProcessor
    {
        public Bitmap Crop(Bitmap source)
        {
            int sourceWidth = source.Width;
            int sourceHeight = source.Height;
            System.Drawing.Rectangle cropArea = new System.Drawing.Rectangle(1, 1, sourceWidth - 2, sourceHeight - 2);
            Bitmap res = source.Clone(cropArea, source.PixelFormat);
            return res;
        }

        public Bitmap Resize(Bitmap source, int width, int height,
            System.Drawing.Drawing2D.InterpolationMode mode = 
            System.Drawing.Drawing2D.InterpolationMode.Default)
        {
            Bitmap res = new Bitmap(width, height);
            using (Graphics gr=Graphics.FromImage(res))
            {
                gr.InterpolationMode = mode;
                gr.DrawImage(source, 0, 0, width, height);
            }
            return res;
        }

        public Bitmap Expand(Bitmap source)
        {
            Bitmap res = new Bitmap(source.Width + 2, source.Height + 2);
            using (Graphics gr = Graphics.FromImage(res))
            {
                gr.Clear(System.Drawing.Color.Transparent);
                gr.DrawImage(source, 1, 1);
            }
            return res;
        }

        //TODO:Extract method with x,y parametrs for SetPixel
        public void Draw9PatchStamp(PatchFrame ptFrame, Bitmap expandedImage)
        {
            //horizontal-top
            int total = 0;
            List<int> top = ptFrame.GetTop();
            for (int i = 0; i < top.Count - 1; i++)//resizingMatrix[0].Count - 1; i++)
            {
                if (i % 2 == 1)
                {
                    for (int j = 0; j < top[i]; j++)
                    {
                        {
                            expandedImage.SetPixel(total, 0, System.Drawing.Color.Black);
                            total++;
                        }
                    }
                }
                else
                {
                    total += top[i];//resizingMatrix[0][i];
                }
            }

            //vertical-right
            total = 0;
            List<int> right = ptFrame.GetRight();
            for (int i = 0; i < right.Count - 1; i++)
            {
                if (i % 2 == 1)
                {
                    for (int j = 0; j < right[i]; j++)
                    {
                        {
                            expandedImage.SetPixel(expandedImage.Width - 1, total, System.Drawing.Color.Black);
                            total++;
                        }
                    }
                }
                else
                {
                    total += right[i];
                }
            }
            //horizontal-bottom
            total = 0;
            List<int> bottom = ptFrame.GetBottom();
            for (int i = 0; i < bottom.Count - 1; i++)
            {
                if (i % 2 == 1)
                {
                    for (int j = 0; j < bottom[i]; j++)
                    {
                        {
                            expandedImage.SetPixel(total, expandedImage.Height - 1, System.Drawing.Color.Black);
                            total++;
                        }
                    }
                }
                else
                {
                    total += bottom[i];
                }
            }

            //vertical-left
            total = 0;
            List<int> left = ptFrame.GetLeft();
            for (int i = 0; i < left.Count - 1; i++)
            {
                if (i % 2 == 1)
                {
                    for (int j = 0; j < left[i]; j++)
                    {
                        {
                            expandedImage.SetPixel(0, total, System.Drawing.Color.Black);
                            total++;
                        }
                    }
                }
                else
                {
                    total += left[i];
                }
            }
        }
    }
}
