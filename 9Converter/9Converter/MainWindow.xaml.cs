using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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
            bool[][] frameMatrix = new bool[4][];
            frameMatrix[0] = new bool[sourceWidth]; //horizontal-top
            frameMatrix[1] = new bool[sourceHeight];//vertical-right
            frameMatrix[2] = new bool[sourceWidth]; //horizontal-bottom
            frameMatrix[3] = new bool[sourceHeight];//vertical-left

            #region Fill frameMatrix
            for (int i = 0; i < frameMatrix[0].Length; i++)
            {
                if (Source.GetPixel(0, i) == System.Drawing.Color.Black)
                    frameMatrix[0][i] = true;
            }

            for (int i = 0; i < frameMatrix[1].Length; i++)
            {
                if (Source.GetPixel(i, sourceWidth-1) == System.Drawing.Color.Black)
                    frameMatrix[1][i] = true;
            }

            for (int i = 0; i < frameMatrix[2].Length; i++)
            {
                if (Source.GetPixel(sourceHeight-1, i) == System.Drawing.Color.Black)
                    frameMatrix[2][i] = true;
            }

            for (int i = 0; i < frameMatrix[3].Length; i++)
            {
                if (Source.GetPixel(i, 0) == System.Drawing.Color.Black)
                    frameMatrix[2][i] = true;
            }
            #endregion

            #region Create resizingMatrix6, 4 and 3
            bool [][] resizingMatrix6=new bool[4][];
            for (int i = 0; i < sourceWidth*6/8; i++)
            {
                
            }
            #endregion
        }
    }
}
