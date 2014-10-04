using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interferometry.math_classes
{
    class RemainderTheoremImplementator
    {
        private readonly int M = 1;
        private readonly List<int> Mi;
        private readonly List<int> MiInverted;
        
        public RemainderTheoremImplementator(List<int> someNumbers)
        {
            foreach (int currentSineNumber in someNumbers)
            {
                M *= currentSineNumber;
            }

            Mi = new List<int>(someNumbers.Count);
            MiInverted = new List<int>(someNumbers.Count);

            for (int i = 0; i < someNumbers.Count; i++)
            {
                Mi.Add(M / someNumbers[i]);

                for (int desiredValue = 0; ; desiredValue++)
                {
                    int temp = desiredValue * Mi[i];

                    if (temp % someNumbers[i] == 1)
                    {
                        MiInverted.Add(desiredValue);
                        break;
                    }
                }
            }
        }

        public long getSolution(List<long> input)
        {
            int resultPoint = 0;
            int i = 0;

            foreach (int currentInt in input)
            {
                resultPoint += currentInt * Mi[i] * MiInverted[i];
                i++;
            }

            return resultPoint;
        }

        public int getM()
        {
            return M;
        }
    }
}
