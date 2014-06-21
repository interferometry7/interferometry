using System;
using System.ComponentModel;

namespace Interferometry.math_classes
{
    [Serializable()]
    public class ZArrayDescriptor
    {
        public long[][] array;
        public int width;
        public int height;

        public ZArrayDescriptor()
        {
        }

        public ZArrayDescriptor(ZArrayDescriptor descriptorToCopy)
        {
            if (descriptorToCopy == null)
            {
                return;
            }

            array = new long[descriptorToCopy.width] [];

            for (int i = 0; i < descriptorToCopy.width; i++)
            {
                array[i] = new long[descriptorToCopy.height];
            }

            width = descriptorToCopy.width;
            height = descriptorToCopy.height;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    array[i][j] = descriptorToCopy.array[i][j];
                }
            }
        }

        public ZArrayDescriptor(String fileName)
        {
            ZArrayDescriptor descriptorFromFile = FilesHelper.readDescriptorWithName(fileName);

            if (descriptorFromFile == null)
            {
                return;
            }

            array = descriptorFromFile.array;
            width = descriptorFromFile.width;
            height = descriptorFromFile.height;
        }

        public void add(ZArrayDescriptor someDescriptor)
        {
            for (int i = 0; i < Math.Min(width, someDescriptor.width); i++)
            {
                for (int j = 0; j < Math.Min(height, someDescriptor.height); j++)
                {
                    array[i][j] = someDescriptor.array[i][j];
                }
            }
        }
    }
}
