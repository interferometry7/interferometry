using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Size = System.Drawing.Size;

namespace Interferometry
{
    class FilesHelper
    {
        public static BitmapImage loadImege()
        {
            OpenFileDialog dialog1 = new OpenFileDialog();
            bool? result = dialog1.ShowDialog();

            if (result == true)
            {
                BitmapImage newBitmapImage = new BitmapImage();

                newBitmapImage.BeginInit();
                newBitmapImage.UriSource = new Uri(dialog1.FileName);
                newBitmapImage.EndInit();                
                return newBitmapImage;
            }

            return null;


            
            
        }
    }
}
