using System;
using System.Diagnostics;
using System.Linq;

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

        public const string DefaultSampleFileName = "gc-test";
        public const double SampleRate = 256;

        private SensorDevice sensors;

        /// <summary>
        /// Transform simple data points (double value) to a Oxyplot chart LineSeries object.
        /// </summary>
        /// <param name="data">Data to be displayed.</param>
        /// <returns>Oxyplot LieneSeries object to be displayed.</returns>
        private static LineSeries SensorDataToLineSeries(double[] data)
        {
            var lineSeries = new LineSeries();
            for (int i = 0; i < data.Length; ++i)
            {
                lineSeries.Points.Add(new DataPoint(i / SampleRate, data[i]));
            }
            return lineSeries;
        }

        /// <summary>
        /// Get Oxyplot LineSeries object form a sensor data file.
        /// </summary>
        /// <param name="fileName">Name of the data file from where the samples should be loaded.</param>
        /// <param name="useRelativePath">Use either relative or absolute path reference.</param>
        /// <returns></returns>
        public static LineSeries GetPulseSeries(string fileName, bool useRelativePath = true)
        {
            var dataSource = new SensorDataFileStreamer((useRelativePath) ? Environment.CurrentDirectory + fileName : fileName);
            return SensorDataToLineSeries(
                SensorDeviceUtil.GetPulseFilteredData(
                SensorDeviceUtil.GetPulseRawData(dataSource.ReadSamples().ToArray(), dataSource)));
        }

        /// <summary>
        /// Create a new SensorModel instance and automatically connect to the hardware device.
        /// </summary>
        public SensorsModel()
        {
            if (sensors != null)
            {
                Logger.Info("Sensors already started!");
                return;
            }

            try
            {
                // get selected com port
                Logger.Info("Selected sensors COM Port: " + SettingsViewModel.Instance.SensorComPort);
                // connect to hardware
                sensors = new SensorDevice(SettingsViewModel.Instance.SensorComPort);
                // set default sensor export file name
                sensors.SetSdFilename(DefaultSampleFileName);
            }
            catch (Exception e)
            {
                const string message = "Could not connect to sensor device!";
                Logger.Error(message, e);
            }
        }

        /// <summary>
        /// Start the sensor data recording.
        /// </summary>
        public bool StartRecording()
        {
          
            if (sensors != null)
            {
                try
                {
                    foreach (var s in sensors.Sensors)
                    {
                        sensors.SetSensorEnabled(s);
                    }
                    
                    sensors.StartRecording();
                    Logger.Info("Sensors recording started!");
                }
                catch (Exception e)
                {
                    const string message = "Sensors recording failed!";
                    Logger.Error(message, e);
                    ShowSensorError(message);
                    return false;
                }
                return true;
            }
            else
            {
                const string message = "Sensors recording could not be started!";
                Logger.Warn(message);
                ShowSensorError(message);
                return false;
            }
        }

        /// <summary>
        /// Stop a current recording instance.
        /// </summary>
        public void StopRecording()
        {
            if (sensors != null)
            {
                sensors.StopRecording();
                sensors.Dispose();
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

        /// <summary>
        /// Close sensor recording and communication instance.
        /// Disconnect from device.
        /// </summary>
        public void Close()
        {
            if (sensors != null)
            {
                sensors.StopRecording();
                sensors.Dispose();
                Logger.Info("Sensors recording stopped!");
            }
        }

        /// <summary>
        /// Show a default sensor error message to the UI.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        private void ShowSensorError(string message)
        {
            MainViewModel.Instance.ShowDialogMessage("Sensor Device Error", message);
        }

    }
}
