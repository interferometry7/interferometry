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
            RemainderTheoremImplementator theoremImplementator = new RemainderTheoremImplementator(sineNumbers);

            int firstImageMax = (int)Utils.maxFromArray(someImages[0]);
            int secondImageMax = (int)Utils.maxFromArray(someImages[1]);

            ZArrayDescriptor resultDescriptor = new ZArrayDescriptor(secondImageMax + 1, firstImageMax + 1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    List<long> currentImageValues = new List<long>();

                    for(int i = 0; i < someImages.Count; i++)
                    {
                        ZArrayDescriptor currentDescriptor = someImages[i];
                        long currentPhase = currentDescriptor.array[x][y];
                        currentImageValues.Add(currentPhase);
                    }

                    //long resultPoint = theoremImplementator.getSolution(currentImageValues);
                    resultDescriptor.array[currentImageValues[1]][currentImageValues[0]] = 1;//resultPoint;
                }
            }

            doWorkEventArgs.Result = resultDescriptor;
        }
    }
}
