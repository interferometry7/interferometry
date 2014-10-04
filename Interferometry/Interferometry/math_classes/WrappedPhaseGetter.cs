using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Interferometry.math_classes
{
    class WrappedPhaseGetter : BackgroundWorker
    {
        private readonly List<String> imagesPath;
        private readonly double[] phaseShifts;
        private readonly int imagesWidth;
        private readonly int imagesHeight;
        private readonly int scaleTo;

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public WrappedPhaseGetter(List<String> imagesPath, int imagesWidth, int imagesHeight, int scaleTo)
        {
            this.imagesWidth = imagesWidth;
            this.imagesHeight = imagesHeight;
            this.imagesPath = imagesPath;
            this.scaleTo = scaleTo;

            phaseShifts = new double[imagesPath.Count];
            double step = 360.0 / imagesPath.Count;

            for (int i = 0; i < phaseShifts.Count(); i++)
            {
                phaseShifts[i] = step * i;
            }

            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
            DoWork += processImage;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void processImage(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            if ((CancellationPending == true))
            {
                doWorkEventArgs.Cancel = true;
                return;
            }

            double[] cArray = new double[phaseShifts.Count()];

            for (int i = 0; i < cArray.Count(); i++)
            {
                cArray[i] = Math.Cos(phaseShifts[i] * (Math.PI /180.0));
            }

            double[] sArray = new double[phaseShifts.Count()];

            for (int i = 0; i < sArray.Count(); i++)
            {
                sArray[i] = Math.Sin(phaseShifts[i] * (Math.PI / 180.0));
            }

            //матрица трансформации для получения ортогонального вектора
            int[][] transformationMatrix = Utils.createIntArray(phaseShifts.Length, phaseShifts.Length);
            buildTransformationMatrix(transformationMatrix);

            double[] sinComponents = new double[phaseShifts.Length];
            double[] cosComponents = new double[phaseShifts.Length];

            for (int i = 0; i < phaseShifts.Length; i++)
            {
                double sSum = 0;
                double cSum = 0;

                for (int j = 0; j < phaseShifts.Length; j++)
                {
                    double matrixComponent = transformationMatrix[i][j];
                    double sArrayComponent = sArray[j];
                    double cArrayComponent = cArray[j];

                    sSum += matrixComponent * sArrayComponent;
                    cSum += matrixComponent * cArrayComponent;
                }

                sinComponents[i] = sSum;
                cosComponents[i] = cSum;
            }

            double[][] sinResults = Utils.createDoubleArray(imagesWidth, imagesHeight);
            double[][] cosResults = Utils.createDoubleArray(imagesWidth, imagesHeight);

            for (int i = 0; i < imagesPath.Count; i++)
            {
                ZArrayDescriptor currentDerscriptor = new ZArrayDescriptor(imagesPath[i]);

                for (int x = 0; x < currentDerscriptor.width; x++)
                {
                    for (int y = 0; y < currentDerscriptor.height; y++)
                    {
                        long currentImageIntencity = currentDerscriptor.array[x][y];
                        sinResults[x][y] += currentImageIntencity * sinComponents[i];
                        cosResults[x][y] += currentImageIntencity * cosComponents[i];
                    }
                }
            }

            long[][] result = Utils.createLongArray(imagesWidth, imagesHeight);

            for (int x = 0; x < imagesWidth; x++)
            {
                for (int y = 0; y < imagesHeight; y++)
                {
                    double atanResult = Math.Atan2(sinResults[x][y], cosResults[x][y]);
                    //double atanResultDegrees = (atanResult * 180.0) / Math.PI;
                    //atanResultDegrees *= scaleTo;

                    result[x][y] = (long)(((atanResult + Math.PI) / (2.0 * Math.PI)) * scaleTo);
                }
            }

            ZArrayDescriptor resultDescriptor = new ZArrayDescriptor();
            resultDescriptor.array = result;
            resultDescriptor.width = imagesWidth;
            resultDescriptor.height = imagesHeight;

            doWorkEventArgs.Result = resultDescriptor;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void buildTransformationMatrix(int[][] transformationMatrix)
        {
            int initialOnePosition = 1;
            int initialMinusOnePosition = phaseShifts.Length - 1;

            for (int i = 0; i < phaseShifts.Length; i++)
            {
                transformationMatrix[initialOnePosition][i] = 1;
                transformationMatrix[initialMinusOnePosition][i] = -1;

                initialOnePosition++;

                if (initialOnePosition > phaseShifts.Length - 1)
                {
                    initialOnePosition -= phaseShifts.Length;
                }

                initialMinusOnePosition++;

                if (initialMinusOnePosition > phaseShifts.Length - 1)
                {
                    initialMinusOnePosition -= phaseShifts.Length;
                }
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
