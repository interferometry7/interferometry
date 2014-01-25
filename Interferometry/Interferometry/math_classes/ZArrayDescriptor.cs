using System;

namespace Interferometry.math_classes
{
    [Serializable()]
    public class ZArrayDescriptor
    {
        public Int64[,] array;
        public int width;
        public int height;

        public ZArrayDescriptor()
        {
        }

        public ZArrayDescriptor(ZArrayDescriptor descriptorToCopy)
        {
            array = new long[descriptorToCopy.width, descriptorToCopy.height];
            width = descriptorToCopy.width;
            height = descriptorToCopy.height;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    array[i, j] = descriptorToCopy.array[i, j];
                }
            }
        }
    }
}
