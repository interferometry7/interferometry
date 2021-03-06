﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Canon.Eos.Framework;
using Canon.Eos.Framework.Eventing;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Threading;
using System.IO;

public delegate void ImageReceived(Image newImage);  

namespace rab1
{
    class ImageGetter
    {
        public event ImageReceived imageReceived;

        private readonly FrameworkManager _manager;
        private EosCamera camera;
        private bool singleShotInProgress;
        private bool cameraLoaded;

        //Birth
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ImageGetter()
        {
            _manager = new FrameworkManager();
            _manager.CameraAdded+=ManagerOnCameraAdded;
            loadFunction();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void ManagerOnCameraAdded(object sender, EventArgs eventArgs)
        {
            loadCameras();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        //Public Methods
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void getImage()
        {
            if (singleShotInProgress == true)
            {
                return;
            }

            singleShotInProgress = true;
            safeCall(() => { if (camera != null)  camera.TakePicture(); }, ex => MessageBox.Show(ex.ToString()));
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




        //Inner Methods
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void loadFunction()
        {
            StartUp();

            if (cameraLoaded == false)
            {
                loadCameras();
                cameraLoaded = true;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void StartUp()
        {
            safeCall(() => { _manager.LoadFramework(); }, ex => { MessageBox.Show(ex.ToString());});
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void loadCameras()
        {
            IEnumerable<EosCamera> cameras = _manager.GetCameras();

            if (cameras != null)
            {
                foreach (var _camera in cameras)
                {
                    _camera.PictureTaken += imageTaken;
                    camera = _camera;
                }
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void safeCall(Action action, Action<Exception> exceptionHandler)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                exceptionHandler(ex);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void imageTaken(object sender, EosImageEventArgs e)
        {
            if (singleShotInProgress == true)
            {
                singleShotInProgress = false;
            }

            //изображение получено
            imageReceived(e.GetImage());
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
