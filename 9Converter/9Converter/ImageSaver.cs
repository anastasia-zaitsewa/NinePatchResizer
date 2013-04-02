using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;

namespace _9Converter
{
    public class ImageSaver
    {
        private enum Dpi
        {
            xhdpi, hdpi, mdpi, ldpi
        }
        
        private string GetImagePath(string path, Dpi dpi)
        {
            string[] splitPath = path.Split('\\');
            int len = splitPath.Length;
            string[] splitPathDpi = new string[len];
            splitPath.CopyTo(splitPathDpi, 0);
            splitPathDpi[len - 1] = "drawable-" + dpi.ToString() + @"\" + splitPath[len - 1];
            string fn = CancelSplitString(splitPathDpi);
            return fn;
        }

        private string CancelSplitString(string[] split)
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

        public void NonLockingSave(Bitmap btm, string filename, ImageFormat format)
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
                var img = Image.FromStream(ms);
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

        public void SaveImageArray(Bitmap[] imgArr, string fn)
        {
            ImageFormat format = ImageFormat.Png;
            if (fn.ToLowerInvariant().EndsWith(".jpeg") || fn.ToLowerInvariant().EndsWith(".jpg"))
            {
                format = ImageFormat.Jpeg;
            }
            else if (fn.ToLowerInvariant().EndsWith(".bmp"))
            {
                format = ImageFormat.Bmp;
            }
            string fnXhdpi = GetImagePath(fn, Dpi.xhdpi);
            if (File.Exists(fnXhdpi))
            {
                File.Delete(fnXhdpi);
            }
            File.Copy(fn, fnXhdpi);
            for (int i = 1; i < 4; i++)
            {
                NonLockingSave(imgArr[i], GetImagePath(fn, (Dpi)i), format);   
            }
        }

    }
}
