using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using NLog;
using OxyPlot;
using OxyPlot.Axes;
using Phrike.GroundControl.Model;

namespace Phrike.GroundControl.ViewModels
{
    class AnalysisViewModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private PlotModel sensorPulsePlotModel;

        public PlotModel SensorPulsePlotModel
        {
            get { return sensorPulsePlotModel; }
            private set { sensorPulsePlotModel = value; }
        }

        public AnalysisViewModel()
        {
            SensorPulsePlotModel = new PlotModel { Title = "Pulse Result", PlotAreaBackground = OxyColors.Transparent };
            SensorPulsePlotModel.LegendBorder = OxyColors.Transparent;
            
            var yAxis = new LinearAxis(AxisPosition.Left, 60, 180, "Pulse (bpm)");
            yAxis.MinorStep = 5;
            yAxis.MajorStep = 20;

            var xAxis = new LinearAxis(AxisPosition.Bottom, 0, Double.NaN, "Time (min)");
            xAxis.MinorStep = 5;
            xAxis.MajorStep = 10;

            this.SensorPulsePlotModel.Axes.Add(yAxis);
            this.SensorPulsePlotModel.Axes.Add(xAxis);
        }

        public async void LoadPulseData()
        {
            try
            {
                var dlg = new OpenFileDialog
                {
                    Filter = "Binary files (*.bin)|*.bin",
                    InitialDirectory = Environment.CurrentDirectory
                };

                if (dlg.ShowDialog() == true)
                {
                    await Task.Run(() =>
                    {
                        var lineSeries = SensorsModel.GetPulseSeries(dlg.FileName, false);
                        lineSeries.Color = OxyColors.Blue;
                        lineSeries.StrokeThickness = 2;
                        lineSeries.Smooth = true;
                        lineSeries.MarkerFill = OxyColors.SteelBlue;

                        SensorPulsePlotModel.Series.Add(lineSeries);
                        SensorPulsePlotModel.InvalidatePlot(true);
                        Logger.Info("Pluse Data successfully loaded!");
                    });
                }
            }
            catch (Exception e)
            {
                Logger.Error("Could not load pulse data!", e);
            }
        }
    }
}
