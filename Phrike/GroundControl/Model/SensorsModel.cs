using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

using OxyPlot;
using OxyPlot.Series;
using Phrike.GMobiLab;

namespace Phrike.GroundControl.Model
{
    class SensorsModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public const double SampleRate = 256;

        private SensorDevice sensors;

        private static LineSeries SensorDataToLineSeries(double[] data)
        {
            LineSeries lineSeries = new LineSeries();
            for (int i = 0; i < data.Length; ++i)
            {
                lineSeries.Points.Add(new DataPoint(i / SampleRate, data[i]));
            }
            return lineSeries;
        }

        public static Series GetPulseSeries(string fileName, bool useRelativePath = true)
        {
            return SensorsModel.SensorDataToLineSeries(
                SensorDeviceUtil.GetPulseFilteredData(
                SensorDeviceUtil.GetPulseRawData(
                SensorDeviceUtil.GetSamples((useRelativePath) ? Environment.CurrentDirectory + fileName : fileName))));
        }

        public SensorsModel()
        {
            if (sensors != null)
            {
                Logger.Info("Sensors already started!");
                return;
            }

            try
            {
                sensors = new SensorDevice("COM7:");
                sensors.SetSdFilename("gc-test");
            }
            catch (Exception e)
            {
                Logger.Error("Could not connect to sensor device!", e.Message);
            }
        }

        public void StartRecording()
        {
            try
            {
                sensors.StartRecording();
                Logger.Info("Sensors recording started!");
            }
            catch (Exception e)
            {
                Logger.Error("Sensors recording failed!", e.Message);
            }
        }

        public void StopRecording()
        {
            if (sensors != null)
            {
                sensors.StopRecording();
                sensors = null;
                Logger.Info("Sensors recording stopped!");
            }
            else
            {
                Logger.Info("Sensors recording is not running!");
            }
        }

        public void Close()
        {
            if (sensors != null)
            {
                sensors.StopRecording();
                sensors.Dispose();
                Logger.Info("Sensors recording stopped!");
            }
        }

    }
}
