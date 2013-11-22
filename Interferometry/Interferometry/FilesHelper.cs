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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MessageBox = System.Windows.MessageBox;
using Size = System.Drawing.Size;

namespace Interferometry
{
    class FilesHelper
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static BitmapImage loadImege()
        {
            /*System.Windows.Forms.OpenFileDialog dialog1 = new System.Windows.Forms.OpenFileDialog();
            bool? result = dialog1.ShowDialog();

            if (result == true)
            {
                BitmapImage newBitmapImage = new BitmapImage();

                newBitmapImage.BeginInit();
                newBitmapImage.UriSource = new Uri(dialog1.FileName);
                newBitmapImage.EndInit();                
                return newBitmapImage;
            }

            return null;*/


            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Stream myStream;
                    if ((myStream = openFileDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            BitmapImage newBitmapImage = new BitmapImage();

                            newBitmapImage.BeginInit();
                            newBitmapImage.UriSource = new Uri(openFileDialog.FileName);
                            newBitmapImage.EndInit();
                            return newBitmapImage;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    return null;
                }
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
        public static void saveImage(ImageSource someImage)
        {
            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)someImage));

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Stream stream;
                    if ((stream = saveFileDialog.OpenFile()) != null)
                    {
                        encoder.Save(stream);
                        stream.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not write file to disk. Original error: " + ex.Message);
                }
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
