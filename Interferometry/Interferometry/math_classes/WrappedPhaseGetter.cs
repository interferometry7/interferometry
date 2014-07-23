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

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public WrappedPhaseGetter(List<String> imagesPath, double[] phaseShifts)
        {
            this.imagesPath = imagesPath;
            this.phaseShifts = phaseShifts;

            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
            DoWork += processImage;
            RunWorkerCompleted += OnRunWorkerCompleted;
            ProgressChanged += OnProgressChanged;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void processImage(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            /*for (int i = 1; (i <= 10); i++)
            {
                System.Threading.Thread.Sleep(500);
                ReportProgress((i * 10));
            }*/

            if ((CancellationPending == true))
            {
                doWorkEventArgs.Cancel = true;
                return;
            }

            //TODO do smth

            /*int width = -1;
            int height = -1;

            for (int i = 0; i < images.Count(); i++)
            {
                if (width == -1)
                {
                    width = images[i].width;
                    height = images[i].height;
                }
                else
                {
                    if ((width != images[i].width) || (height != images[i].height))
                    {
                        throw new Exception("Размеры изображений не совпадают");
                    }
                }
            }*/

            double[] cArray = new double[phaseShifts.Count()];

            for (int i = 0; i < cArray.Count(); i++)
            {
                cArray[i] = Math.Cos(phaseShifts[i]);
            }

            double[] sArray = new double[phaseShifts.Count()];

            for (int i = 0; i < sArray.Count(); i++)
            {
                sArray[i] = Math.Sin(phaseShifts[i]);
            }



            phaseShifts = new double[4];
            cArray = new []{1.0,2.0,3.0, 4.0};
            sArray = new[] { 1.0, 2.0, 3.0 , 4.0};

            int[][] matrixForInvertVectorCounting = new int[phaseShifts.Length][];

            for (int i = 0; i < phaseShifts.Length; i++)
            {
                matrixForInvertVectorCounting[i] = new int[phaseShifts.Length];
            }

            int initialOnePosition = phaseShifts.Length - 2;
            int initialMinusOnePosition = phaseShifts.Length - 1;

            for (int i = 0; i < phaseShifts.Length; i++)
            {
                matrixForInvertVectorCounting[initialOnePosition][i] = 1;
                matrixForInvertVectorCounting[initialMinusOnePosition][i] = -1;

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

            double[] reverseSArray = new double[phaseShifts.Length];
            double[] reverseCArray = new double[phaseShifts.Length];

            for (int i = 0; i < phaseShifts.Length; i++)
            {
                double cSum = 0;
                double sSum = 0;

                for (int j = 0; j < phaseShifts.Length; j++)
                {
                    cSum += matrixForInvertVectorCounting[j][i]*sArray[j];
                    sSum += matrixForInvertVectorCounting[j][i]*cArray[j];
                }

                reverseSArray[i] = sSum;
                reverseCArray[i] = cSum;
            }


            //проверка
            double sumS = 0;
            double sumC = 0;
            for (int i = 0; i < phaseShifts.Length; i++)
            {
                sumS += sArray[i]*reverseSArray[i];
                sumC += cArray[i] * reverseCArray[i];
            }
            //проверка




            doWorkEventArgs.Result = null;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void OnProgressChanged(object sender, ProgressChangedEventArgs progressChangedEventArgs)
        {
            String progress = (progressChangedEventArgs.ProgressPercentage + "%");
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void OnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {

        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
