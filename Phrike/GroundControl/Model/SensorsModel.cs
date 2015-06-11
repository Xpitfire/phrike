using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OperationPhrike.GMobiLab;
using OxyPlot;
using OxyPlot.Series;

namespace Phrike.GroundControl.Model
{
    public static class SensorsModel
    {
        public const double SampleRate = 256;

        private static LineSeries SensorDataToLineSeries(double[] data)
        {
            LineSeries lineSeries = new LineSeries();
            for (int i = 0; i < data.Length; ++i)
            {
                lineSeries.Points.Add(new DataPoint(i / SampleRate, data[i]));
            }
            return lineSeries;
        }

        public static Series GetPulseSeries()
        {
            return SensorsModel.SensorDataToLineSeries(
                SensorDeviceUtil.GetPulseFilteredData(
                SensorDeviceUtil.GetPulseRawData(
                SensorDeviceUtil.GetSamples(Environment.CurrentDirectory + "/Samples/EKG-Signal_zur_Verarbeitung_23_04_15.bin"))));
        }
    }
}
