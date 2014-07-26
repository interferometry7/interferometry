using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Interferometry.math_classes
{
    class WrappedPhaseGetter : BackgroundWorker
    {
        private List<String> imagesPath;
        private double[] phaseShifts;
        private int imagesWidth;
        private int imagesHeight;

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public WrappedPhaseGetter(List<String> imagesPath, int imagesWidth, int imagesHeight)
        {
            this.imagesWidth = imagesWidth;
            this.imagesHeight = imagesHeight;
            this.imagesPath = imagesPath;

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

            int[][] transformationMatrix = new int[phaseShifts.Length][];

            for (int i = 0; i < phaseShifts.Length; i++)
            {
                transformationMatrix[i] = new int[phaseShifts.Length];
            }

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

            double[][] sinResults = new double[imagesWidth][];

            for (int i = 0; i < imagesWidth; i++)
            {
                sinResults[i] = new double[imagesHeight];
            }

            double[][] cosResults = new double[imagesWidth][];

            for (int i = 0; i < imagesWidth; i++)
            {
                cosResults[i] = new double[imagesHeight];
            }


            for (int i = 0; i < imagesPath.Count; i++)
            {
                ZArrayDescriptor currentDerscriptor = new ZArrayDescriptor(imagesPath[i]);

                for (int x = 0; x < currentDerscriptor.width; x++)
                {
                    for (int y = 0; y < currentDerscriptor.height; y++)
                    {
                        long currentImageIntencity = currentDerscriptor.array[x][y];
                        sinResults[x][y] += currentImageIntencity*sinComponents[i];
                        cosResults[x][y] += currentImageIntencity * cosComponents[i];
                    }
                }
            }

            long[][] result = new long[imagesWidth][];

            for (int i = 0; i < imagesWidth; i++)
            {
                result[i] = new long[imagesHeight];
            }

            double min = Double.MaxValue;
            double max = Double.MinValue;

            long minDegree = long.MaxValue;
            long maxDegree = long.MinValue;

            for (int x = 0; x < imagesWidth; x++)
            {
                for (int y = 0; y < imagesHeight; y++)
                {
                    double atanResult = Math.Atan2(sinResults[x][y], cosResults[x][y]);

                    min = Math.Min(min, atanResult);
                    max = Math.Max(max, atanResult);

                    result[x][y] = (long) (atanResult*180/Math.PI + 180.0);

                    minDegree = Math.Min(minDegree, result[x][y]);
                    maxDegree = Math.Max(maxDegree, result[x][y]);
                }
            }

            ZArrayDescriptor resultDescriptor = new ZArrayDescriptor();
            resultDescriptor.array = result;
            resultDescriptor.width = imagesWidth;
            resultDescriptor.height = imagesHeight;

            doWorkEventArgs.Result = resultDescriptor;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
