using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Interferometry.interfaces
{
    public interface ImageContainerDelegate
    {
        void exportImage(ImageContainer imageContainer, ImageSource someImage);
        ImageSource getImageToLoad(ImageContainer imageContainer);
    }
}
