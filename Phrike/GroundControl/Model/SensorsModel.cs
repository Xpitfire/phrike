using System;
using NLog;
using OxyPlot;
using OxyPlot.Series;
using Phrike.GMobiLab;
using Phrike.GroundControl.ViewModels;

namespace Phrike.GroundControl.Model
{
    class SensorsModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public const double SampleRate = 256;

        private SensorDevice sensors;

        private static LineSeries SensorDataToLineSeries(double[] data)
        {
            var lineSeries = new LineSeries();
            for (int i = 0; i < data.Length; ++i)
            {
                lineSeries.Points.Add(new DataPoint(i / SampleRate, data[i]));
            }
            return lineSeries;
        }

        public static LineSeries GetPulseSeries(string fileName, bool useRelativePath = true)
        {
            return SensorDataToLineSeries(
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
                const string message = "Could not connect to sensor device!";
                Logger.Error(message, e.Message);
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
                const string message = "Sensors recording failed!";
                Logger.Error(message, e);
                ShowSensorError(message);
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
                const string message = "Sensors recording is not running!";
                Logger.Warn(message);
                ShowSensorError(message);
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

        private void ShowSensorError(string message)
        {
            MainViewModel.Instance.ShowDialogMessage("Sensor Device Error", message);
        }

    }
}
