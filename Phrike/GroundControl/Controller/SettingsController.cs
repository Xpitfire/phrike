using Phrike.GroundControl.Models;
using System.Collections.Generic;
using System.IO.Ports;

namespace Phrike.GroundControl.Controller
{
    public static class SettingsController
    {
        public static IEnumerable<Sensor> Sensors { get; private set; }

        public static IEnumerable<string> COMPorts { get; private set; }

        static SettingsController()
        {
            LoadSensors();
            LoadCOMPorts();
        }

        private static void LoadSensors()
        {
            Sensors = new List<Sensor>()
            {
                new Sensor(SensorType.None, "Kein Sensor"), 
                new Sensor(SensorType.GMobiLab, "g.MOBIlab+"),
                new Sensor(SensorType.Biofeedback, "Biofeedback 2000 x-pert")
            };
        }

        private static void LoadCOMPorts()
        {
            COMPorts = SerialPort.GetPortNames();
        }
    }
}
