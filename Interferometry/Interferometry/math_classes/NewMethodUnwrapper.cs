using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Interferometry.math_classes;


namespace Interferometry.math_classes
{
    class NewMethodUnwrapper : BackgroundWorker
    {
        private List<ZArrayDescriptor> someImages;
        private List<int> sineNumbers;
        private int M = 1;
        private List<int> Mi;
        private List<int> MiInverted;

        public NewMethodUnwrapper(List<ZArrayDescriptor> someImages, List<int> sineNumbers)
        {
            this.someImages = someImages;
            this.sineNumbers = sineNumbers;

            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
            DoWork += processImages;
        }

        private void processImages(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            int width = someImages[0].width;
            int height = someImages[0].height;

            //описание - https://ru.wikipedia.org/wiki/%D0%9A%D0%B8%D1%82%D0%B0%D0%B9%D1%81%D0%BA%D0%B0%D1%8F_%D1%82%D0%B5%D0%BE%D1%80%D0%B5%D0%BC%D0%B0_%D0%BE%D0%B1_%D0%BE%D1%81%D1%82%D0%B0%D1%82%D0%BA%D0%B0%D1%85

            foreach (int currentSineNumber in sineNumbers)
            {
                M *= currentSineNumber;
            }

            Mi = new List<int>(sineNumbers.Count);
            MiInverted = new List<int>(sineNumbers.Count);

            for(int i = 0; i < sineNumbers.Count; i++)
            {
                Mi.Add(M / sineNumbers[i]);

                for (int desiredValue = 0; ;desiredValue++)
                {
                    int temp = desiredValue*Mi[i];

                    if (temp%sineNumbers[i] == 1)
                    {
                        MiInverted.Add(desiredValue);
                        break;
                    }
                }
            }
            
            ZArrayDescriptor resultDescriptor = new ZArrayDescriptor();
            resultDescriptor.width = 1000;
            resultDescriptor.height = 1000;
            resultDescriptor.array = new long[resultDescriptor.width][];

            for (int i = 0; i < resultDescriptor.width; i++)
            {
                resultDescriptor.array[i] = new long[resultDescriptor.height];
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    long resultPoint = 0;
                    List<long> currentImageValues = new List<long>();

                    for(int i = 0; i < someImages.Count; i++)
                    {
                        ZArrayDescriptor currentDescriptor = someImages[i];
                        long currentPhase = currentDescriptor.array[x][y];
                        currentImageValues.Add(currentPhase);
                        resultPoint += (currentPhase%sineNumbers[i]) * Mi[i] * MiInverted[i];
                    }

                    resultDescriptor.array[currentImageValues[0]] [currentImageValues[1]] = resultPoint;
                }
            }

            doWorkEventArgs.Result = resultDescriptor;
        }
    }
}
