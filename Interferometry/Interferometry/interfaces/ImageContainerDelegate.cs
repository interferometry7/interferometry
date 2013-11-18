using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace Interferometry.interfaces
{
    public interface ImageContainerDelegate
    {
        void exportImage(ImageContainer imageContainer, BitmapImage bitmapImage);
        BitmapImage getImageToLoad(ImageContainer imageContainer);
    }
}
