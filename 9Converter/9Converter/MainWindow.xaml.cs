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
using System.Threading.Tasks;
using System.Threading;


namespace _9Converter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> FileList = new List<string>();

        public MainWindow()
        {
            
            InitializeComponent();
            txtHint.Visibility = Visibility.Visible;
            progressBar1.Visibility = Visibility.Hidden;
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

        private void ImageProcessing()
        {
            lstbDragAndDrop.AllowDrop = false;
            CheckDirsInList();

            string path;
            List<string> imageExts = new List<string> { ".jpeg", ".jpg", ".png", ".bmp" };
            Bitmap[] result=null;

            progressBar1.Minimum = 0;
            progressBar1.Maximum = FileList.Count;
            progressBar1.Value = 0;
            progressBar1.Visibility = Visibility.Visible;
            Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < FileList.Count; i++)
                {
                    path = FileList[i];
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

                    Dispatcher.Invoke( new Action(()=>{ progressBar1.Value++; }), null);
                }
                FileList.Clear();
                Dispatcher.Invoke(new Action( () =>
                    {
                        MessageBox.Show(Application.Current.MainWindow,"Check Folder with Your Image(s).", "9 Patch Resizer: \"Resizing succed.\"");
                        txtHint.Text = "DragDrop Your Images or 9Patches";
                        progressBar1.Visibility = Visibility.Hidden;
                        lstbDragAndDrop.AllowDrop = true;
                    }), null);
            });
        }

        private void CheckDirsInList()
        {
            string path;
            for (int i = 0; i < FileList.Count; i++)
            {
                path = FileList[i];
                string[] allFiles;
                if (Directory.Exists(path))
                {
                    allFiles = Directory.GetFiles(path);
                    FileList.RemoveAt(i);
                    for (int j = 0; j < allFiles.Length; j++)
                    {
                        FileList.Add(allFiles[j]);
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
            txtHint.Text = "";
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                FileList.Add(file);
            }

            ImageProcessing();
        }
        #endregion
    }
}
