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

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            CheckDirsInList();

            string path;
            List<string> imageExts = new List<string> { ".jpeg", ".jpg", ".png", ".bmp" };
            Bitmap[] result=null;

            for (int i = 0; i < lstbDragAndDrop.Items.Count; i++)
            {
                path = lstbDragAndDrop.Items[i].ToString();
                result = null;
                if (path.ToLowerInvariant().EndsWith(".9.png"))
                {
                    NinePatchResizer nRes = new NinePatchResizer();
                    result = nRes.ResizeImage(path);
                }
                else if (imageExts.Contains(System.IO.Path.GetExtension(path).ToLowerInvariant()))
                {
                    ImageResizer imRes = new ImageResizer();
                    result = imRes.ResizeImage(path);
                }
                CreateDir(path);
                if (result != null)
                {
                    ImageSaver imS = new ImageSaver();
                    //TODO: Do not retrive Sourse in Bitmap[]
                    imS.SaveImageArray(result, path);
                }
            }
            lstbDragAndDrop.Items.Clear();
            MessageBox.Show("Check Folder with Your Image(s).", "Resizing succed.");
        }

        private void CheckDirsInList()
        {
            string path;
            for (int i = 0; i < lstbDragAndDrop.Items.Count; i++)
            {
                path = lstbDragAndDrop.Items[i].ToString();
                string[] allFiles;
                if (Directory.Exists(path))
                {
                    allFiles = Directory.GetFiles(path);
                    lstbDragAndDrop.Items.RemoveAt(i);
                    for (int j = 0; j < allFiles.Length; j++)
                    {
                        lstbDragAndDrop.Items.Add(allFiles[j]);
                    }
                    i--;
                }
            }
        }

        #region Drag & Drop
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
        #endregion

        private void progressBar1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
