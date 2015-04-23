using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OperationPhrike.GMobiLab;

namespace OperationPhrike.SensorDeviceTest
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                // connect to g.MOBIlab+
                //string comPort = "COM";
                Console.WriteLine("Bitte Port zum Verbinden eingeben: ");
                //comPort += Console.ReadLine();

                Console.WriteLine("[g.tec] try to connect to g.MOBIlab+");
                var sensorDevice = new SensorDevice("COM6:"); // comPort += ":"

                // enable channels
                int[] channels = { 1, 2, 3 };
                Console.WriteLine("[g.tec] enable channels");
                sensorDevice.SetAnalogChannelsEnabled(channels, true);

                // set filename
                Console.WriteLine("Bitte Filenamen eingeben (null für deaktivieren von SDCard): ");
                //string fileName = Console.ReadLine();
                sensorDevice.SetSdFilename("test"); //fileName
                Console.WriteLine("[g.tec] filename set");

                Console.ReadLine();

            } 
            catch (GMobiLabException ex)
            {
                Console.WriteLine("[ERROR] " + ex.Message);
            }
            
           
        }
    }
}
