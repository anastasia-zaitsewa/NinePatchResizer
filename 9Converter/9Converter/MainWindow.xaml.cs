﻿using System;
using System.Collections.Generic;
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
using System.Drawing;

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

            #region Crop Source Image
            System.Drawing.Rectangle cropArea = new System.Drawing.Rectangle(1, 1, 161, 97);
            Bitmap cropSource = Source.Clone(cropArea, Source.PixelFormat);
            string filename=@"C:\Users\Настя\Documents\Projects\nineConverter\spinner_cut.9.png";
            NonLockingSave(cropSource, filename, ImageFormat.Png);
            #endregion
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
    }
}
