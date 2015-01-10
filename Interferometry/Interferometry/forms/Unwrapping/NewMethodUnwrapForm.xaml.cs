using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Interferometry.math_classes;

public delegate void PhaseUnwrappedWithNewMethod(ZArrayDescriptor result);

namespace Interferometry.forms.Unwrapping
{
    /// <summary>
    /// Interaction logic for NewMethodUnwrapForm.xaml
    /// </summary>
    public partial class NewMethodUnwrapForm : Window
    {
        private ZArrayDescriptor firstPhase;
        private ZArrayDescriptor secondPhase;
        private ZArrayDescriptor table;

        public event PhaseUnwrappedWithNewMethod phaseUnwrappedWithNewMethod;

        public NewMethodUnwrapForm(ZArrayDescriptor firstPhase, ZArrayDescriptor secondPhase, ZArrayDescriptor table)
        {
            InitializeComponent();

            this.table = table;
            this.secondPhase = secondPhase;
            this.firstPhase = firstPhase;

           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ZArrayDescriptor result = new ZArrayDescriptor(firstPhase.width, firstPhase.height);

            for (int x = 0; x < firstPhase.width; x++)
            {
                for (int y = 0; y < firstPhase.height; y++)
                {
                    long firstPhaseValue = firstPhase.array[x][y];
                    long secondPhaseValue = secondPhase.array[x][y];
                    long currentMaskValue = table.array[secondPhaseValue][firstPhaseValue];

                    int lineNumber = (int)(currentMaskValue / 10);

                    //Console.WriteLine("lineNumber = " + lineNumber);

                    //if ((lineNumber <= 9) && (lineNumber > 0))
                    {
                        result.array[x][y] = secondPhaseValue + 241*(lineNumber - 1);
                    }
                }
            }

            if (phaseUnwrappedWithNewMethod != null)
            {
                phaseUnwrappedWithNewMethod(result);
            }
        }
    }
}
