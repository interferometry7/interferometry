using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Interferometry.math_classes
{
    /// <summary>
    /// Класс реализует алгоритм утоньшения линий Zhang Suen. Описание - http://www-prima.inrialpes.fr/perso/Tran/Draft/gateway.cfm.pdf  
    /// </summary>
    class ZhangSuenAlgorithmImplementator : BackgroundWorker
    {
        private ZArrayDescriptor input;

        //Life Cycle
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ZhangSuenAlgorithmImplementator(ZArrayDescriptor input)
        {
            this.input = input;

            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
            DoWork += processImage;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void processImage(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            ZArrayDescriptor result = process(input);
            doWorkEventArgs.Result = result;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        //Public Methods
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static ZArrayDescriptor process(ZArrayDescriptor inputArray)
        {
            ZArrayDescriptor result = new ZArrayDescriptor(inputArray.width, inputArray.height);

            for (int x = 1; x < inputArray.width - 1; x++)
            {
                for (int y = 1; y < inputArray.height - 1; y++)
                {
                    long P9 = inputArray.array[x - 1][y - 1];
                    long P2 = inputArray.array[x][y - 1];
                    long P3 = inputArray.array[x + 1][y - 1];
                    long P4 = inputArray.array[x + 1][y];
                    long P5 = inputArray.array[x + 1][y + 1];
                    long P6 = inputArray.array[x][y + 1];
                    long P7 = inputArray.array[x - 1][y + 1];
                    long P8 = inputArray.array[x - 1][y];
                    long P1 = inputArray.array[x][y];

                    List<long> parameter = new List<long>(8);
                    parameter.Add(P2);
                    parameter.Add(P3);
                    parameter.Add(P4);
                    parameter.Add(P5);
                    parameter.Add(P6);
                    parameter.Add(P7);
                    parameter.Add(P8);
                    parameter.Add(P9);

                    long A = ZhangSuenAlgorithmImplementator.A(parameter);
                    long B = ZhangSuenAlgorithmImplementator.B(parameter);

                    if (((B >= 2) && (B <= 6)) 
                        && (A == 1) 
                        && (P2*P4*P6 == 0)
                        && (P4 * P6 * P8 == 0))
                    {
                        P1 = 0;
                    }
                    else
                    {
                        P1 = 2;
                    }

                    if (((B >= 2) && (B <= 6))
                        && (A == 1)
                        && (P2 * P4 * P8 == 0)
                        && (P2 * P6 * P8 == 0))
                    {
                        P1 = 0;
                    }
                    else
                    {
                        P1 = 2;
                    }
                }
            }

            return result;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
         
         
       
        //Private Methods
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static int A(List<long> P)
        {
            int result = 0;
            int i = 0;

            foreach (long currentValue in P)
            {
                if (i != P.Count - 1)
                {
                    long nextValue = P[i + 1];

                    if ((currentValue == 0) && (nextValue == 1))
                    {
                        result++;
                    }
                }

                i++;
            }

            return result;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static long B(List<long> P)
        {
            long result = 0;

            foreach (long currentValue in P)
            {
                result += currentValue;
            }

            return result;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
