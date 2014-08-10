using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using Interferometry.forms;

namespace Interferometry.math_classes
{
    class LissajousImageBuilder : BackgroundWorker
    {
        private List<String> imagesPath;
        private double[] phaseShifts;
        private int imagesWidth;
        private int imagesHeight;

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public LissajousImageBuilder(List<String> imagesPath, int imagesWidth, int imagesHeight)
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

            for (int x = 0; x < imagesWidth; x++)
            {
                for (int y = 0; y < imagesHeight; y++)
                {
                    sinResults[x][y] = sinResults[x][y] /** 180.0 / Math.PI*/;
                    cosResults[x][y] = cosResults[x][y] /** 180.0 / Math.PI*/;
                }
            }


            double maxSin = Double.MinValue;
            double maxCos = Double.MinValue;

            double minSin = Double.MaxValue;
            double minCos = Double.MaxValue;



            for (int x = 0; x < imagesWidth; x++)
            {
                for (int y = 0; y < imagesHeight; y++)
                {
                    maxSin = Math.Max(maxSin, sinResults[x][y]);
                    maxCos = Math.Max(maxCos, cosResults[x][y]);

                    minSin = Math.Min(minSin, sinResults[x][y]);
                    minCos = Math.Min(minCos, cosResults[x][y]);
                }
            }

            int resultWidth = (int) (maxCos - minCos) + 1;
            int resultHeight = (int)(maxSin - minSin) + 1;

            long[][] result = new long[resultWidth][];

            for (int i = 0; i < resultWidth; i++)
            {
                result[i] = new long[resultHeight];
            }

            for (int x = 0; x < imagesWidth; x++)
            {
                for (int y = 0; y < imagesHeight; y++)
                {
                    int sin = (int) sinResults[x][y];
                    int cos = (int) cosResults[x][y];

                    int currentSin = (int) ((int) sinResults[x][y] - minSin);
                    int currentCos = (int) ((int) cosResults[x][y] - minCos);
                    result[currentCos][currentSin]+= 1;
                }
            }



            ZArrayDescriptor resultDescriptor = new ZArrayDescriptor();
            resultDescriptor.array = result;
            resultDescriptor.width = resultWidth;
            resultDescriptor.height = resultHeight;

            doWorkEventArgs.Result = resultDescriptor;
        }
    }
}
