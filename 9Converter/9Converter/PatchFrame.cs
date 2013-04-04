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
    public class PatchFrame
    {
        //fields
        private List<int>[] frameMatrix;

        //constructors
        public PatchFrame()
        {
            frameMatrix = new List<int>[4];

            frameMatrix[0] = new List<int>();
            frameMatrix[1] = new List<int>();
            frameMatrix[2] = new List<int>();
            frameMatrix[3] = new List<int>();
        }

        //methods
        public void Fill(Bitmap source)
        {
            int sourceWidth = source.Width;
            int sourceHeight = source.Height;
            //horizontal-top
            System.Drawing.Color curColor = source.GetPixel(0, 0);
            int count = 0;

            for (int i = 0; i < sourceWidth; i++)
            {
                if (source.GetPixel(i, 0) == curColor)
                {
                    count++;
                }
                else
                {
                    (frameMatrix[0]).Add(count);
                    curColor = source.GetPixel(i, 0);
                    count = 1;
                }
            }
            frameMatrix[0].Add(count);

            //vertical-right
            curColor = source.GetPixel(sourceWidth - 1, 0);
            count = 0;

            for (int i = 0; i < sourceHeight; i++)
            {
                if (source.GetPixel(sourceWidth - 1, i) == curColor)
                {
                    count++;
                }
                else
                {
                    frameMatrix[1].Add(count);
                    curColor = source.GetPixel(sourceWidth - 1, i);
                    count = 1;
                }
            }
            frameMatrix[1].Add(count);

            //horizontal-bottom
            curColor = source.GetPixel(0, sourceHeight - 1);
            count = 0;

            for (int i = 0; i < sourceWidth; i++)
            {
                if (source.GetPixel(i, sourceHeight - 1) == curColor)
                {
                    count++;
                }
                else
                {
                    frameMatrix[2].Add(count);
                    curColor = source.GetPixel(i, sourceHeight - 1);
                    count = 1;
                }
            }
            frameMatrix[2].Add(count);

            //vertical-left
            curColor = source.GetPixel(0, 0);
            count = 0;

            for (int i = 0; i < sourceHeight; i++)
            {
                if (source.GetPixel(0, i) == curColor)
                {
                    count++;
                }
                else
                {
                    frameMatrix[3].Add(count);
                    curColor = source.GetPixel(0, i);
                    count = 1;
                }
            }
            frameMatrix[3].Add(count);
        }

        public PatchFrame Resize(int k)
        {
            int res;
            PatchFrame resFrame = new PatchFrame();

            for (int i = 0; i < frameMatrix.Length; i++)
            {
                for (int j = 0; j < frameMatrix[i].Count; j++)
                {
                    res = k * frameMatrix[i][j] / 8;

                    if (res < 1)
                    {
                        resFrame.frameMatrix[i].Add(1);
                    }
                    else
                    {
                        if (k * frameMatrix[i][j] % 8 < 5)
                        {
                            resFrame.frameMatrix[i].Add(res);
                        }
                        else
                        {
                            resFrame.frameMatrix[i].Add(res + 1);
                        }
                    }
                }
            }
            return resFrame;
        }

        private List<int> GetList(int index)
        {
            List<int> res = new List<int>();
            res.AddRange(frameMatrix[index]);
            return res;
        }

        public List<int> GetTop()
        {
            return GetList(0);
        }

        public List<int> GetRight()
        {
            return GetList(1);
        }

        public List<int> GetBottom()
        {
            return GetList(2);
        }

        public List<int> GetLeft()
        {
            return GetList(3);
        }
    }
}
