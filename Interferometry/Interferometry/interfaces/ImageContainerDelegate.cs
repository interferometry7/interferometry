using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using rab1;

namespace Interferometry.interfaces
{
    public interface ImageContainerDelegate
    {
        void exportImage(ImageContainer imageContainer, Pi_Class1.ZArrayDescriptor arrayDescriptor);
        ImageSource getImageToLoad(ImageContainer imageContainer);
    }
}
