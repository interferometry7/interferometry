﻿using System;
using System.Collections.Generic;
using System.Linq;
using Canon.Eos.Framework;
using Canon.Eos.Framework.Eventing;

namespace rab1
{
    public sealed class FrameworkManager
    {
        private EosFramework _framework;

        public event EventHandler CameraAdded;

        public IEnumerable<EosCamera> GetCameras()
        {
            if (_framework != null)
            {
                using (var cameras = _framework.GetCameraCollection())
                    return cameras.ToArray();
            }
            else
            {
                return null;
            }
        }

        public void LoadFramework()
        {
            if (_framework == null)
            {
                _framework = new EosFramework();
                _framework.CameraAdded += this.HandleCameraAdded;
            }
        }

        public void ReleaseFramework()
        {
            if (_framework != null)
            {
                _framework.CameraAdded -= this.HandleCameraAdded;
                _framework.Dispose();
            }
        }        

        private void HandleCameraAdded(object sender, EventArgs eventArgs)
        {
            if (this.CameraAdded != null)
                this.CameraAdded(this, eventArgs);
        }
    }
}
