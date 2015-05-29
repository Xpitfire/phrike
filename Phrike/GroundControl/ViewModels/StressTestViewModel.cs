using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phrike.GroundControl.ViewModels
{
    public class StressTestViewModel
    {

        public void StartUnrealEngine()
        {
            System.Diagnostics.Process.Start(@"/ext/hello-world.bat");
        }

        public void StartMyo()
        {
            System.Diagnostics.Process.Start(@"/ext/hello-world.bat");
        }

    }
}
