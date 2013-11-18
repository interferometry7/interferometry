using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Size = System.Drawing.Size;

namespace Interferometry
{
    class FilesHelper
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static Bitmap bitmapImageToBitmap(BitmapImage bitmapImage)
        {
            if (bitmapImage == null)
            {
                return null;
            }

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                Bitmap bitmap = new Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /*public static BitmapImage bitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                ms.Position = 0;
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                ms.Seek(0, SeekOrigin.Begin);
                bi.StreamSource = ms;
                bi.EndInit();

                return bi;
            }
        }*/
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);

        public static BitmapSource bitmapToBitmapImage(Bitmap source)
        {
            IntPtr ip = source.GetHbitmap();
            BitmapSource bs;
            try
            {
                bs = Imaging.CreateBitmapSourceFromHBitmap(ip,
                   IntPtr.Zero, Int32Rect.Empty,
                   BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(ip);
            }

            return bs;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
