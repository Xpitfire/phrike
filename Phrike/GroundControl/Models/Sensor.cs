using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phrike.GroundControl.Models
{
    public class Sensor
    {
        public int SensorType { get; set; }

        public string SensorName { get; set; }

        public Sensor(int sensorType, string sensorName)
        {
            SensorType = sensorType;
            SensorName = sensorName;
        }
    }
}
