using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Interferometry.math_classes;

namespace Interferometry.interfaces
{
    public interface ImageContainerDelegate
    {
        void exportImage(ImageContainer imageContainer, ZArrayDescriptor arrayDescriptor);
        ZArrayDescriptor getImageToLoad(ImageContainer imageContainer);
    }
}
