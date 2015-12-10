using Phrike.GroundControl.Controller;

namespace Phrike.GroundControl.Models
{
    public enum SensorType
    {
        None = 0,
        GMobiLab = 1,
        Biofeedback = 2
    }

    public class Sensor
    {
        public SensorType SensorType { get; set; }

        public string SensorName { get; set; }

        public Sensor(SensorType sensorType, string sensorName)
        {
            SensorType = sensorType;
            SensorName = sensorName;
        }
    }
}
