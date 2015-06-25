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
using OxyPlot.Series;
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

        /// <summary>
        /// Create a new analysis viemodel instance and add the default plot template.
        /// </summary>
        public AnalysisViewModel()
        {
            SensorPulsePlotModel = new PlotModel { Title = "Pulse Result", PlotAreaBackground = OxyColors.Transparent };
            SensorPulsePlotModel.LegendBorder = OxyColors.Transparent;
            
            var yAxis = new LinearAxis(AxisPosition.Left, 0, 200, "Pulse (bpm)");
            yAxis.MinorStep = 5;
            yAxis.MajorStep = 20;
            yAxis.MajorGridlineStyle = LineStyle.Solid;
            yAxis.MajorGridlineColor = OxyColors.LightGray;
            yAxis.MinorGridlineStyle = LineStyle.Solid;
            yAxis.MinorGridlineColor = OxyColor.FromRgb(240, 240, 240);

            var xAxis = new LinearAxis(AxisPosition.Bottom, 0, Double.NaN, "Time (sec)");
            xAxis.MinorStep = 5;
            xAxis.MajorStep = 10;

            this.SensorPulsePlotModel.Axes.Add(yAxis);
            this.SensorPulsePlotModel.Axes.Add(xAxis);
        }

        /// <summary>
        /// Load pulse data from a selected file and add to the plot view.
        /// </summary>
        public async void LoadPulseData()
        {
            try
            {
                // start a file dialog and select a sensor file
                var dlg = new OpenFileDialog
                {
                    Filter = "Binary files (*.bin)|*.bin",
                    InitialDirectory = Environment.CurrentDirectory
                };

                if (dlg.ShowDialog() == true)
                {
                    await Task.Run(() =>
                    {
                        SensorPulsePlotModel.Series.Clear();
                        // create a line series plot instance
                        var lineSeries = SensorsModel.GetPulseSeries(dlg.FileName, false);
                        lineSeries.Color = OxyColors.Blue;
                        lineSeries.StrokeThickness = 2;
                        lineSeries.Smooth = true;
                        
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
