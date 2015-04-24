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
                // comPort += ":"
                using (var sensorDevice = new SensorDevice("COM6:"))
                {

                    // enable channels
                    int[] channels = {0, 2, 3, 4, 5, 6 };
                    Console.WriteLine("[g.tec] enable channels");
                    sensorDevice.SetAnalogChannelsEnabled(channels, true);

                    // set filename
                    Console.WriteLine("Bitte Filenamen eingeben (null für deaktivieren von SDCard): ");
                    //string fileName = Console.ReadLine();
                    sensorDevice.SetSdFilename("test_24_04_15"); //fileName
                    Console.WriteLine("[g.tec] filename set");



                    sensorDevice.StartRecordingData();
                    Console.WriteLine("[g.tec] recording started");

                    Console.WriteLine("\npress enter for stopping recroding process...");
                    Console.ReadLine();

                    sensorDevice.StopRecordingData();
                    Console.WriteLine("[g.tec] recording stopped");

                }

                Console.ReadLine();

            } 
            catch (GMobiLabException ex)
            {
                Console.WriteLine("[ERROR] " + ex.Message);
                Console.ReadLine();
            }
            
           
        }
    }
}
