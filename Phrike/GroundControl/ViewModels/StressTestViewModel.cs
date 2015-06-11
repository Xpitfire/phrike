using Phrike.GroundControl.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Phrike.GroundControl.ViewModels
{

    class StressTestViewModel
    {

        public void StartUnrealEngine()
        {
            ProcessModel.StartProcess("/ext/unreal-engine.bat");
        }

        public void StartSensors()
        {

        }

        public void StartScreenCapture()
        {

        }

    }
}
