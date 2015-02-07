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
            ZArrayDescriptor copyArray = new ZArrayDescriptor(inputArray);

            for (int x = 0; x < copyArray.width; x++)
            {
                for (int y = 0; y < copyArray.height; y++)
                {
                    if (inputArray.array[x][y] > 0)
                    {
                        copyArray.array[x][y] = 1;
                    }
                    else
                    {
                        copyArray.array[x][y] = 0;
                    }

                    result.array[x][y] = 1;
                }
            }

            List<long> parameter = new List<long>(8);

            for (;;)
            {
                Boolean smthDeleted = false;

                for (int x = 0; x < copyArray.width; x++)
                {
                    for (int y = 0; y < copyArray.height; y++)
                    {
                        result.array[x][y] = 1;
                    }
                }

                for (int x = 1; x < inputArray.width - 1; x++)
                {
                    for (int y = 1; y < inputArray.height - 1; y++)
                    {
                        long P1 = copyArray.array[x][y];

                        if (P1 == 0)
                        {
                            continue;
                        }

                        long P9 = copyArray.array[x - 1][y - 1];
                        long P2 = copyArray.array[x][y - 1];
                        long P3 = copyArray.array[x + 1][y - 1];
                        long P4 = copyArray.array[x + 1][y];
                        long P5 = copyArray.array[x + 1][y + 1];
                        long P6 = copyArray.array[x][y + 1];
                        long P7 = copyArray.array[x - 1][y + 1];
                        long P8 = copyArray.array[x - 1][y];

                        parameter.Clear();
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
                            && (P4*P6*P8 == 0))
                        {
                            result.array[x][y] = 0;
                            smthDeleted = true;
                        }
                    }
                }

                for (int x = 0; x < copyArray.width; x++)
                {
                    for (int y = 0; y < copyArray.height; y++)
                    {
                        if (result.array[x][y] == 0)
                        {
                            copyArray.array[x][y] = 0;
                        }
                    }
                }

                for (int x = 0; x < copyArray.width; x++)
                {
                    for (int y = 0; y < copyArray.height; y++)
                    {
                        result.array[x][y] = 1;
                    }
                }

                for (int x = 1; x < inputArray.width - 1; x++)
                {
                    for (int y = 1; y < inputArray.height - 1; y++)
                    {
                        long P1 = copyArray.array[x][y];

                        if (P1 == 0)
                        {
                            continue;
                        }

                        long P9 = copyArray.array[x - 1][y - 1];
                        long P2 = copyArray.array[x][y - 1];
                        long P3 = copyArray.array[x + 1][y - 1];
                        long P4 = copyArray.array[x + 1][y];
                        long P5 = copyArray.array[x + 1][y + 1];
                        long P6 = copyArray.array[x][y + 1];
                        long P7 = copyArray.array[x - 1][y + 1];
                        long P8 = copyArray.array[x - 1][y];

                        parameter.Clear();
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
                                 && (P2 * P4 * P8 == 0)
                                 && (P2 * P6 * P8 == 0))
                        {
                            result.array[x][y] = 0;
                            smthDeleted = true;
                        }
                    }
                }

                for (int x = 0; x < copyArray.width; x++)
                {
                    for (int y = 0; y < copyArray.height; y++)
                    {
                        if (result.array[x][y] == 0)
                        {
                            copyArray.array[x][y] = 0;
                        }
                    }
                }

                //if (smthDeleted == false)
                {
                    break;
                }
            }

            return copyArray;
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
