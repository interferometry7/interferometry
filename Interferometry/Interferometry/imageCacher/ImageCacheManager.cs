using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Interferometry.math_classes;

namespace Interferometry.imageCacher
{
    class ImageCacheManager
    {
        private int widthNumber;
        private int heightNumber;
        private List<ImageCacheUnit> units;
        private const int MAX_PIECE_DIMENSION = 1000;
        private List<String> pathes;

        public int getWidth()
        {
            return widthNumber;
        }

        public int getHeight()
        {
            return heightNumber;
        }

        public int getImageNumber()
        {
            if (pathes != null)
            {
                return pathes.Count;
            }

            return 0;
        }

        public ZArrayDescriptor[] getArray(int x, int y)
        {
            int planeNumber = 0;

            for (int i = 0; i < widthNumber; i++)
            {
                for (int j = 0; j < heightNumber; j++)
                {
                    if ((i == x) && (j == y))
                    {
                        ImageCacheUnit neededUnit = units[planeNumber];

                        if (Environment.Is64BitProcess == false)
                        {
                            return loadDescriptorsFromFiles(neededUnit);
                        }
                        else
                        {
                            return null;
                        }
                    }

                    planeNumber++;
                }
            }

            return null;
        }

        private ZArrayDescriptor[] loadDescriptorsFromFiles(ImageCacheUnit neededUnit)
        {
            ZArrayDescriptor[] result = new ZArrayDescriptor[pathes.Count];
            int counter = 0;

            foreach (String currentPath in pathes)
            {
                ZArrayDescriptor savedArray = new ZArrayDescriptor(currentPath);

                ZArrayDescriptor newArray = new ZArrayDescriptor();
                newArray.width = neededUnit.width;
                newArray.height = neededUnit.height;
                newArray.array = new long[neededUnit.width, neededUnit.height];

                for (int i = 0; i < neededUnit.width; i++)
                {
                    for (int j = 0; j < neededUnit.height; j++)
                    {
                        newArray.array[i, j] = savedArray.array[i + neededUnit.xStart, j + neededUnit.yStart];
                    }
                }

                result[counter] = newArray;
                counter++;
            }

            return result;
        }

        public void setZArrayDescriptors(List<ZArrayDescriptor> newDescriptors)
        {

        }

        public void setFilePathes(List<String> newPathes, int imageWidth, int imageHeight)
        {
            pathes = newPathes;
            int xPieces = imageWidth / MAX_PIECE_DIMENSION;
            int yPieces = imageHeight / MAX_PIECE_DIMENSION;

            if (xPieces * MAX_PIECE_DIMENSION != imageWidth)
            {
                xPieces++;
            }

            if (yPieces * MAX_PIECE_DIMENSION != imageHeight)
            {
                yPieces++;
            }

            widthNumber = xPieces;
            heightNumber = yPieces;

            units = new List<ImageCacheUnit>(widthNumber * heightNumber);

            for (int i = 0; i < widthNumber; i++)
            {
                for (int j = 0; j < heightNumber; j++)
                {
                    int xStart = i*MAX_PIECE_DIMENSION;
                    int yStart = j*MAX_PIECE_DIMENSION;
                    int unitWidth;
                    int unitHeight;
                    int widthRest = imageWidth - xStart;
                    int heightRest = imageHeight - yStart;

                    if (widthRest > MAX_PIECE_DIMENSION)
                    {
                        unitWidth = MAX_PIECE_DIMENSION;
                    }
                    else
                    {
                        unitWidth = widthRest;
                    }

                    if (heightRest > MAX_PIECE_DIMENSION)
                    {
                        unitHeight = MAX_PIECE_DIMENSION;
                    }
                    else
                    {
                        unitHeight = heightRest;
                    }

                    ImageCacheUnit newUnit = new ImageCacheUnit();
                    newUnit.width = unitWidth;
                    newUnit.height = unitHeight;
                    newUnit.xStart = xStart;
                    newUnit.yStart = yStart;

                    units.Add(newUnit);
                }
            }
        }

    }
}
