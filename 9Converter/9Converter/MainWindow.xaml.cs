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
            Image9Patch();
        }

        private void Image9Patch()
        {
            Bitmap Source = SourceImageFactory.NonLockingOpen("Something");
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

            #region Work with Files and Directories
            CreateDir("Something");
            string fn = "Something";
            string[] splitPath = fn.Split(@"\"[0]);
            int ln = splitPath.Length;

            string[] splitPathDpi = new string[ln];
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
        }

        private void btnResize_Click(object sender, RoutedEventArgs e)
        {
            SimpleImage();
        }

        private void SimpleImage()
        {
            Bitmap Source = SourceImageFactory.NonLockingOpen("Something");
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

            #region Work with Files and Directories
            CreateDir("Something");
            string fn = "Something";
            string[] splitPath = fn.Split(@"\"[0]);
            int ln = splitPath.Length;

            string[] splitPathDpi = new string[ln];
            splitPath.CopyTo(splitPathDpi, 0);
            splitPathDpi[ln - 1] = @"drawable-xhdpi\" + splitPath[ln - 1];
            fn = CancelSplitString(splitPathDpi);
            NonLockingSave(Source, fn, ImageFormat.Png);

            splitPath.CopyTo(splitPathDpi, 0);
            splitPathDpi[ln - 1] = @"drawable-hdpi\" + splitPath[ln - 1];
            fn = CancelSplitString(splitPathDpi);
            NonLockingSave(resizingImage6, fn, ImageFormat.Png);

            splitPath.CopyTo(splitPathDpi, 0);
            splitPathDpi[ln - 1] = @"drawable-mdpi\" + splitPath[ln - 1];
            fn = CancelSplitString(splitPathDpi);
            NonLockingSave(resizingImage4, fn, ImageFormat.Png);

            splitPath.CopyTo(splitPathDpi, 0);
            splitPathDpi[ln - 1] = @"drawable-ldpi\" + splitPath[ln - 1];
            fn = CancelSplitString(splitPathDpi);
            NonLockingSave(resizingImage3, fn, ImageFormat.Png);
            #endregion
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

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            string[] files = Directory.GetFiles("Something");
           
            for (int i = 0; i < files.Length; i++)
            {
                //txtFileName.Text = files[i];
                SimpleImage();
            }
            MessageBox.Show("Check Folder with Your Image(s).", "Convertation succed.");

        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            string[] files = Directory.GetFiles("Something");
            for (int i = 0; i < files.Length; i++)
            {
                /*txtFileName.Text = files[i];
                Image9Patch();*/
            }
            MessageBox.Show("Check Folder with Your Image(s).", "Convertation succed.");
        }

        private void lstbDragAndDrop_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void lstbDragAndDrop_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                lstbDragAndDrop.Items.Add(file);
            }
        }
    }
}
