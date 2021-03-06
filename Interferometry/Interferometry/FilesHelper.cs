﻿using System;
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
using Interferometry.math_classes;
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
            openFileDialog.Filter = "All files (*.*)|*.*|bmp files (*.bmp)|*.bmp";
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
        public static String getPathToObjFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "obj files (*.obj)|*.obj|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }

            return null;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static ZArrayDescriptor loadZArray()
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
                            ZArrayDescriptor savedArray = (ZArrayDescriptor) deserializer.Deserialize(myStream);
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
        public static BitmapImage[] loadBunchImages()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*|bmp files (*.bmp)|*.bmp";
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
                            String initialFilePath = openFileDialog.FileName;

                            if (initialFilePath.Contains("1") == false)
                            {
                                throw new Exception("Выбрана не первое изображение в серии. В имени файла нет цифры \"1\"");
                            }

                            String initialFileName = Path.GetFileName(initialFilePath);
                            String initialDirectory = Path.GetDirectoryName(initialFilePath);
                            List<BitmapImage> temporartResult = new List<BitmapImage>();

                            for (int i = 0; ; i++)
                            {
                                String currentFileName = initialFileName.Replace("1", "" + (i + 1));
                                String currentFilePath = Path.Combine(initialDirectory, currentFileName);

                                if (File.Exists(currentFilePath) == false)
                                {
                                    break;
                                }
                                
                                BitmapImage newBitmapImage = new BitmapImage();

                                newBitmapImage.BeginInit();
                                newBitmapImage.UriSource = new Uri(currentFilePath);
                                newBitmapImage.EndInit();

                                temporartResult.Add(newBitmapImage);
                            }

                            myStream.Close();
                            BitmapImage[] result = temporartResult.ToArray();
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
        public static ZArrayDescriptor[] loadArrays()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "array files (*.zarr)|*.zarr|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            ZArrayDescriptor[] result = new ZArrayDescriptor[8];

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
                                ZArrayDescriptor loadedDescriptor = (ZArrayDescriptor) deserializer.Deserialize(fileStream);
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

                return bitmap;
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

                return bitmap;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);

        public static BitmapSource bitmapToBitmapImage(Bitmap source)
        {
            if (source == null)
            {
                return null;
            }

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

            bs.Freeze();
            return bs;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void saveImage(ImageSource someImage)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "All files (*.*)|*.*|bmp files (*.bmp)|*.bmp";
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
        public static void saveZArray(ZArrayDescriptor arrayDescriptor)
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
        public static void saveZArrays(ZArrayDescriptor[] arraysToSave)
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
                        ZArrayDescriptor currentDescriptor = arraysToSave[i];
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
        public static void saveDescriptorWithName(ZArrayDescriptor descriptor, String fileName)
        {
            if ((descriptor == null) || (fileName == null))
            {
                return;
            }

            Stream stream = new FileStream(fileName, FileMode.Create);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(stream, descriptor);
            stream.Close();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static ZArrayDescriptor readDescriptorWithName(String fileName)
        {
            try
            {
                Stream stream = new FileStream(fileName, FileMode.Open);
                BinaryFormatter deserializer = new BinaryFormatter();
                ZArrayDescriptor savedArray = (ZArrayDescriptor) deserializer.Deserialize(stream);
                stream.Close();
                return savedArray;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void saveImages(ImageSource[] imagesToSave)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "All files (*.*)|*.*|bmp files (*.bmp)|*.bmp";
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
