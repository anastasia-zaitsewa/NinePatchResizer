using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Drawing.Imaging;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace _9Converter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
            InitializeComponent();
        }

        private void CreateDir(string filename)
        {
            string[] splitPath = filename.Split(@"\"[0]);
            int fnLength = splitPath[splitPath.Length - 1].Length;
            string folderPath = filename.Remove(filename.Length - fnLength);

            if (!Directory.Exists(folderPath + @"\drawable-xhdpi"))
                Directory.CreateDirectory(folderPath + @"\drawable-xhdpi");
            if (!Directory.Exists(folderPath + @"\drawable-hdpi"))
                Directory.CreateDirectory(folderPath + @"\drawable-hdpi");
            if (!Directory.Exists(folderPath + @"\drawable-mdpi"))
                Directory.CreateDirectory(folderPath + @"\drawable-mdpi");
            if (!Directory.Exists(folderPath + @"\drawable-ldpi"))
                Directory.CreateDirectory(folderPath + @"\drawable-ldpi");
        }

        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            Bitmap Source = SourceImageFactory.NonLockingOpen(txtFileName.Text);
            int sourceWidth = Source.Width;
            int sourceHeight = Source.Height;

            List<int>[] frameMatrix = new List<int>[4];
            frameMatrix[0] = new List<int>();
            frameMatrix[1] = new List<int>();
            frameMatrix[2] = new List<int>();
            frameMatrix[3] = new List<int>();

            #region Fill frameMatrix
            //horizontal-top
            System.Drawing.Color curColor = Source.GetPixel(0, 0);
            int count = 0;

            for (int i = 0; i < sourceWidth; i++)
            {
                if (Source.GetPixel(i, 0) == curColor)
                {
                    count++;
                }
                else
                {
                    (frameMatrix[0]).Add(count);
                    curColor = Source.GetPixel(i, 0);
                    count = 1;
                }
            }
            frameMatrix[0].Add(count);

            //vertical-right
            curColor = Source.GetPixel(sourceWidth-1, 0);
            count = 0;

            for (int i = 0; i < sourceHeight; i++)
            {
                if (Source.GetPixel(sourceWidth-1, i) == curColor)
                {
                    count++;
                }
                else
                {
                    frameMatrix[1].Add(count);
                    curColor = Source.GetPixel(sourceWidth-1, i);
                    count = 1;
                }
            }
            frameMatrix[1].Add(count);

            //horizontal-bottom
            curColor = Source.GetPixel(0, sourceHeight-1);
            count = 0;

            for (int i = 0; i < sourceWidth; i++)
            {
                if (Source.GetPixel(i, sourceHeight-1) == curColor)
                {
                    count++;
                }
                else
                {
                    frameMatrix[2].Add(count);
                    curColor = Source.GetPixel(i, sourceHeight-1);
                    count = 1;
                }
            }
            frameMatrix[2].Add(count);

            //vertical-left
            curColor = Source.GetPixel(0, 0);
            count = 0;
            
            for (int i = 0; i < sourceHeight; i++)
            {
                if (Source.GetPixel(0, i) == curColor)
                {
                    count++;
                }
                else
                {
                    frameMatrix[3].Add(count);
                    curColor = Source.GetPixel(0, i);
                    count = 1;
                }
            }
            frameMatrix[3].Add(count);
            #endregion

            #region Create resizingMatrix6, 4 and 3
            List<int>[] resizingMatrix6 = Resizing(frameMatrix, 6);
            List<int>[] resizingMatrix4 = Resizing(frameMatrix, 4);
            List<int>[] resizingMatrix3 = Resizing(frameMatrix, 3);
            #endregion

            #region Crop, Resize and Expand Source Image
            //Crop
            System.Drawing.Rectangle cropArea = new System.Drawing.Rectangle(1, 1, 161, 97);
            Bitmap cropSource = Source.Clone(cropArea, Source.PixelFormat);
            
            //Resize
            Bitmap resizingImage6 = ResizeBitmap(cropSource, sourceWidth * 6 / 8, sourceHeight * 6 / 8);
            Bitmap resizingImage4 = ResizeBitmap(cropSource, sourceWidth / 2, sourceHeight / 2);
            Bitmap resizingImage3 = ResizeBitmap(cropSource, sourceWidth * 3 / 8, sourceHeight * 3 / 8, 
                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic);

            /*string fn = @"C:\Users\Настя\Documents\Projects\nineConverter\spinner_resize.9.png";
            NonLockingSave(resizingImage6, fn, ImageFormat.Png);*/
            
            //Expand
            Bitmap expandedImage6 = ExpandBitmap(resizingImage6);
            Bitmap expandedImage4 = ExpandBitmap(resizingImage4);
            Bitmap expandedImage3 = ExpandBitmap(resizingImage3);
            #endregion

            #region Draw Nine_Stamp
            Draw9PatchStamp(resizingMatrix6, expandedImage6);
            Draw9PatchStamp(resizingMatrix4, expandedImage4);
            Draw9PatchStamp(resizingMatrix3, expandedImage3);
            #endregion
            
            #region Work with Files and Directories
            CreateDir(txtFileName.Text);
            string fn=txtFileName.Text;
            string[] splitPath = fn.Split(@"\"[0]);
            int ln = splitPath.Length;

            string[] splitPathDpi=new string[ln];
            splitPath.CopyTo(splitPathDpi, 0);
            splitPathDpi[ln - 1] = @"drawable-xhdpi\" + splitPath[ln - 1];
            fn = CancelSplitString(splitPathDpi);
            NonLockingSave(Source, fn, ImageFormat.Png);

            splitPath.CopyTo(splitPathDpi, 0);
            splitPathDpi[ln - 1] = @"drawable-hdpi\" + splitPath[ln - 1];
            fn = CancelSplitString(splitPathDpi);
            NonLockingSave(expandedImage6, fn, ImageFormat.Png);

            splitPath.CopyTo(splitPathDpi, 0);
            splitPathDpi[ln - 1] = @"drawable-mdpi\" + splitPath[ln - 1];
            fn = CancelSplitString(splitPathDpi);
            NonLockingSave(expandedImage4, fn, ImageFormat.Png);

            splitPath.CopyTo(splitPathDpi, 0);
            splitPathDpi[ln - 1] = @"drawable-ldpi\" + splitPath[ln - 1];
            fn = CancelSplitString(splitPathDpi);
            NonLockingSave(expandedImage3, fn, ImageFormat.Png);
            #endregion
            MessageBox.Show("Check Folder with Your Image(s).", "Convertation succed.");
        }

        private static string CancelSplitString(string[] split)
        {
            string fn = "";
            for (int i = 0; i < split.Length; i++)
            {
                if (i == split.Length - 1)
                {
                    fn += split[i];
                }
                else
                {
                    fn = fn + split[i] + @"\"[0];
                }

            }
            return fn;
        }

        private static void Draw9PatchStamp(List<int>[] resizingMatrix, Bitmap expandedImage)
        {
            //horizontal-top
            int total = 0;
            for (int i = 0; i < resizingMatrix[0].Count - 1; i++)
            {
                if (i % 2 == 1)
                {
                    for (int j = 0; j < resizingMatrix[0][i]; j++)
                    {
                        {
                            expandedImage.SetPixel(total, 0, System.Drawing.Color.Black);
                            total++;
                        }
                    }
                }
                else
                {
                    total += resizingMatrix[0][i];
                }
            }

            //vertical-right
            total = 0;
            for (int i = 0; i < resizingMatrix[1].Count - 1; i++)
            {
                if (i % 2 == 1)
                {
                    for (int j = 0; j < resizingMatrix[1][i]; j++)
                    {
                        {
                            expandedImage.SetPixel(expandedImage.Width - 1, total, System.Drawing.Color.Black);
                            total++;
                        }
                    }
                }
                else
                {
                    total += resizingMatrix[1][i];
                }
            }

            //horizontal-bottom
            total = 0;
            for (int i = 0; i < resizingMatrix[2].Count - 1; i++)
            {
                if (i % 2 == 1)
                {
                    for (int j = 0; j < resizingMatrix[2][i]; j++)
                    {
                        {
                            expandedImage.SetPixel(total, expandedImage.Height - 1, System.Drawing.Color.Black);
                            total++;
                        }
                    }
                }
                else
                {
                    total += resizingMatrix[2][i];
                }
            }

            //vertical-left
            total = 0;
            for (int i = 0; i < resizingMatrix[3].Count - 1; i++)
            {
                if (i % 2 == 1)
                {
                    for (int j = 0; j < resizingMatrix[3][i]; j++)
                    {
                        {
                            expandedImage.SetPixel(0, total, System.Drawing.Color.Black);
                            total++;
                        }
                    }
                }
                else
                {
                    total += resizingMatrix[3][i];
                }
            }
        }

        private static Bitmap ExpandBitmap(Bitmap btm)
        {
            Bitmap result = new Bitmap(btm.Width + 2, btm.Height + 2);
            using (Graphics gr = Graphics.FromImage(result))
            {
                gr.Clear(System.Drawing.Color.Transparent);
                gr.DrawImage(btm, 1, 1);
            }
            return result;
        }

        private static List<int>[] Resizing(List<int>[] frameMatrix, int k)
        {
            int res;
            List<int>[] resizingMatrix = new List<int>[4];
            resizingMatrix[0] = new List<int>();
            resizingMatrix[1] = new List<int>();
            resizingMatrix[2] = new List<int>();
            resizingMatrix[3] = new List<int>();

            for (int i = 0; i < frameMatrix.Length; i++)
            {
                for (int j = 0; j < frameMatrix[i].Count; j++)
                {
                    res = k * frameMatrix[i][j] / 8;

                    if (res < 1)
                    {
                        resizingMatrix[i].Add(1);
                    }
                    else
                    {
                        if (k * frameMatrix[i][j] % 8 < 5)
                        {
                            resizingMatrix[i].Add(res);
                        }
                        else
                        {
                            resizingMatrix[i].Add(res + 1);
                        }
                    }
                }
            }
            return resizingMatrix;
        }

        public static void NonLockingSave(Bitmap btm, string filename, ImageFormat format)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            try
            {
                #region Convert Bitmap to Destination Format
                MemoryStream ms = new MemoryStream();
                Bitmap btm2 = (Bitmap)btm.Clone();
                btm2.Save(ms, format);
                btm2.Dispose();
                byte[] byteArr = ms.ToArray();
                ms.Close();
                ms.Dispose();
                #endregion

                #region Save Byte Array to File
                FileStream fs = new FileStream(filename, FileMode.CreateNew, FileAccess.Write);
                try
                {
                    fs.Write(byteArr, 0, byteArr.Length);
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    fs.Close();
                    fs.Dispose();
                }
                #endregion
            }
            catch (Exception ex)
            {
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
                MessageBox.Show(ex.Message);
            }
        }

        public static Bitmap ResizeBitmap(Bitmap source, int width, int height, 
            System.Drawing.Drawing2D.InterpolationMode mode = 
            System.Drawing.Drawing2D.InterpolationMode.Default)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics gr=Graphics.FromImage(result))
            {
                gr.InterpolationMode = mode;
                gr.DrawImage(source, 0, 0, width, height);
            }
            return result;
        }
    }
}
