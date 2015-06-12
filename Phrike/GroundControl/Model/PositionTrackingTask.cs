using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Phrike.GroundControl.Model
{
    class PositionTrackingTask 
    {

        private UnrealEngineModel unrealEngineModel;

        public PositionTrackingTask(UnrealEngineModel unrealEngineModel)
        {
            this.unrealEngineModel = unrealEngineModel;
            Thread trackingThread = new Thread(new ThreadStart(Run));
            trackingThread.Start();
        }

        public void Run()
        {
            
        }

    }
}
