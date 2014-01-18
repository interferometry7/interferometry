using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using rab1;
using MessageBox = System.Windows.MessageBox;
using Size = System.Drawing.Size;

namespace Interferometry
{
    class FilesHelper
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static BitmapImage loadImage()
        {
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
                            myStream.Close();
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
        public static Pi_Class1.ZArrayDescriptor loadZArray()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "array files (*.zarr)|*.zarr|All files (*.*)|*.*";
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
                            BinaryFormatter deserializer = new BinaryFormatter();
                            Pi_Class1.ZArrayDescriptor savedArray = (Pi_Class1.ZArrayDescriptor) deserializer.Deserialize(myStream);
                            myStream.Close();
                            return savedArray;
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
        public static BitmapImage[] loadEightImages()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            BitmapImage[] result = new BitmapImage[8];

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Stream myStream;
                    if ((myStream = openFileDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                String fileName;

                                if (openFileDialog.FileName.Contains("1."))
                                {
                                    fileName = openFileDialog.FileName.Replace("1.", (i + 1) + ".");
                                }
                                else
                                {
                                    fileName = openFileDialog.FileName;
                                }

                                BitmapImage newBitmapImage = new BitmapImage();

                                newBitmapImage.BeginInit();
                                newBitmapImage.UriSource = new Uri(fileName);
                                newBitmapImage.EndInit();

                                result[i] = newBitmapImage;
                            }

                            return result;
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
        public static Pi_Class1.ZArrayDescriptor[] loadArrays()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "array files (*.zarr)|*.zarr|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            Pi_Class1.ZArrayDescriptor[] result = new Pi_Class1.ZArrayDescriptor[8];

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Stream myStream;
                    if ((myStream = openFileDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                String fileName;

                                if (openFileDialog.FileName.Contains("1."))
                                {
                                    fileName = openFileDialog.FileName.Replace("1.", (i + 1) + ".");
                                }
                                else
                                {
                                    fileName = openFileDialog.FileName;
                                }

                                Stream fileStream = File.OpenRead(fileName);
                                BinaryFormatter deserializer = new BinaryFormatter();
                                Pi_Class1.ZArrayDescriptor loadedDescriptor = (Pi_Class1.ZArrayDescriptor) deserializer.Deserialize(fileStream);
                                fileStream.Close();
                                result[i] = loadedDescriptor;
                            }

                            return result;
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
        public static Bitmap bitmapSourceToBitmap(BitmapSource bitmapImage)
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
                        BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create((BitmapSource)someImage));
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
        public static void saveZArray(Pi_Class1.ZArrayDescriptor arrayDescriptor)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "array files (*.zarr)|*.zarr|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Stream stream;
                    if ((stream = saveFileDialog.OpenFile()) != null)
                    {
                        BinaryFormatter serializer = new BinaryFormatter();
                        serializer.Serialize(stream, arrayDescriptor);
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
        public static void saveZArrays(Pi_Class1.ZArrayDescriptor[] arraysToSave)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "array files (*.zarr)|*.zarr|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Stream stream;
                    int pathLength = saveFileDialog.FileName.Length;
                    String pathWithoutExtension = saveFileDialog.FileName.Substring(0, pathLength - 5);

                    for (int i = 0; i < arraysToSave.Length; i++)
                    {
                        Pi_Class1.ZArrayDescriptor currentDescriptor = arraysToSave[i];
                        saveFileDialog.FileName = pathWithoutExtension + Convert.ToString(i + 1) + ".zarr";

                        if ((stream = saveFileDialog.OpenFile()) != null)
                        {
                            BinaryFormatter serializer = new BinaryFormatter();
                            serializer.Serialize(stream, currentDescriptor);
                            stream.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not write file to disk. Original error: " + ex.Message);
                }
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void saveImages(ImageSource[] imagesToSave)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    int pathLength = saveFileDialog.FileName.Length;
                    String pathWithoutExtension = saveFileDialog.FileName.Substring(0, pathLength - 5);

                    for (int i = 0; i < imagesToSave.Length; i++)
                    {
                        ImageSource currentImage = imagesToSave[i];
                        saveFileDialog.FileName = pathWithoutExtension + Convert.ToString(i + 1) + ".bmp";

                        Stream stream;
                        if ((stream = saveFileDialog.OpenFile()) != null)
                        {
                            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)currentImage));
                            encoder.Save(stream);
                            stream.Close();
                        }
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
