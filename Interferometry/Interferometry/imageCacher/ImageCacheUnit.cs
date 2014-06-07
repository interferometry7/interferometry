using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interferometry.forms;
using Interferometry.math_classes;

namespace Interferometry.imageCacher
{
    class ImageCacheUnit
    {
        public int imageNumber;
        public int xStart;
        public int yStart;
        public int width;
        public int height;

        /*public void setDescriptor(ZArrayDescriptor newDescriptor)
        {
            zArrayDescriptor = newDescriptor;
        }

        public void setFilePath(String newFilePath)
        {
            filePath = newFilePath;
        }*/

        /*public ZArrayDescriptor getDescriptor()
        {
            if (zArrayDescriptor != null)
            {
                ZArrayDescriptor result = new ZArrayDescriptor();
                result.width = width;
                result.height = height;
                result.array = new long[width, height];

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        result.array[i, j] = zArrayDescriptor.array[i + xStart, j + yStart];
                    }
                }

                return result;
            }
            else
            {
                if (filePath == null)
                {
                    return null;
                }

                ZArrayDescriptor descriptorFromFile = new ZArrayDescriptor(filePath);

                ZArrayDescriptor result = new ZArrayDescriptor();
                result.width = width;
                result.height = height;
                result.array = new long[width, height];

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        result.array[i, j] = descriptorFromFile.array[i + xStart, j + yStart];
                    }
                }

                return result;
            }
        }*/
    }
}
