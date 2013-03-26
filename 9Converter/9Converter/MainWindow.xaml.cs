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
            List<int>[] frameMatrix = new List<int>[4];

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
                    frameMatrix[0].Add(count);
                    curColor = Source.GetPixel(i, 0);
                    count = 0;
                }
            }
            frameMatrix[0].Add(count);

            //vertical-right
            curColor = Source.GetPixel(sourceWidth, 0);
            count = 0;

            for (int i = 0; i < sourceHeight; i++)
            {
                if (Source.GetPixel(sourceWidth, i) == curColor)
                {
                    count++;
                }
                else
                {
                    frameMatrix[1].Add(count);
                    curColor = Source.GetPixel(sourceWidth, i);
                    count = 0;
                }
            }
            frameMatrix[1].Add(count);

            //horizontal-bottom
            curColor = Source.GetPixel(0, sourceHeight);
            count = 0;

            for (int i = 0; i < sourceWidth; i++)
            {
                if (Source.GetPixel(i, sourceHeight) == curColor)
                {
                    count++;
                }
                else
                {
                    frameMatrix[2].Add(count);
                    curColor = Source.GetPixel(i, sourceHeight);
                    count = 0;
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
                    count = 0;
                }
            }
            frameMatrix[3].Add(count);
            #endregion

            #region Create resizingMatrix6, 4 and 3
            List<int>[] resizingMatrix6 = new List<int>[4];
            int k = 6;
            for (int i = 0; i < frameMatrix.Length; i++)
            {
                for (int j = 0; j < frameMatrix[i].Count; j++)
                {
                    //frameMatrix[i][j]
                    //resizingMatrix6[i].Add();
                }
            }
            #endregion
        }
    }
}
