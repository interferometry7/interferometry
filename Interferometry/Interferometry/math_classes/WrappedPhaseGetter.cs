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
            RunWorkerAsync();
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

            for (int i = 0; i < cArray.Count(); i++)
            {
                cArray[i] = Math.Sin(phaseShifts[i]);
            }




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
