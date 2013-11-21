using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

public delegate void ImageCaptured(Image newImage); 

namespace rab1
{
    class ShooterSingleton
    {
        public static event ImageCaptured imageCaptured;

        private static ImageGetter imageGetter;
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void init()
        {
            if(imageGetter == null)
            {
                imageGetter = new ImageGetter();
                imageGetter.imageReceived += imageTaken;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static void imageTaken(Image newImage)
        {
            //изображение получено
            imageCaptured(newImage);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void getImage()
        {
            imageGetter.getImage();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
