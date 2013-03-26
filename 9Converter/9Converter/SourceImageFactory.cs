using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace _9Converter
{
    public class SourceImageFactory
    {
        public static Bitmap NonLockingOpen(string filename)
        {
            #region  Save file to byte array
            long size = (new FileInfo(filename)).Length;
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[size];
            try
            {
                fs.Read(data, 0, (int)size);
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

            #region Convert bytes to image

            MemoryStream ms = new MemoryStream();
            ms.Write(data, 0, (int)size);
            Bitmap result = new Bitmap(ms);
            ms.Close();
            #endregion

            return result;
        }
    }
}
