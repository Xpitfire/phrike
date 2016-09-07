using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Axes;

namespace Phrike.GroundControl.Controller
{
    class PlotController
    {
        public string FileFilter => "Binary files (*.bin)|*.bin";

        public PlotModel CreatePlotModel()
        {
            PlotModel sensorPulsPlot = new PlotModel { Title = "Pulse Result", PlotAreaBackground = OxyColors.Transparent };
            sensorPulsPlot.LegendBorder = OxyColors.Transparent;

            var yAxis = new LinearAxis();
            yAxis.Position = AxisPosition.Left;
            yAxis.Minimum = 0;
            yAxis.Maximum = 200;
            yAxis.Title = "Pulse (bpm)";
            yAxis.MinorStep = 5;
            yAxis.MajorStep = 20;
            yAxis.MajorGridlineStyle = LineStyle.Solid;
            yAxis.MajorGridlineColor = OxyColors.LightGray;
            yAxis.MinorGridlineStyle = LineStyle.Solid;
            yAxis.MinorGridlineColor = OxyColor.FromRgb(240, 240, 240);

            var xAxis = new LinearAxis();
            xAxis.Position = AxisPosition.Bottom;
            xAxis.Minimum = 0;
            xAxis.Maximum = double.NaN;
            xAxis.Title = "Time (sec)";
            xAxis.MinorStep = 5;
            xAxis.MajorStep = 10;

            sensorPulsPlot.Axes.Add(yAxis);
            sensorPulsPlot.Axes.Add(xAxis);

            return sensorPulsPlot;
        }

        public void LoadPlotData(PlotModel sensorPulsePlot, string fileName)
        {
            sensorPulsePlot.Series.Clear();
            // create a line series plot instance
            var lineSeries = SensorsController.GetPulseSeries(fileName, false);
            lineSeries.Color = OxyColors.Blue;
            lineSeries.StrokeThickness = 2;
            lineSeries.Smooth = true;

            sensorPulsePlot.Series.Add(lineSeries);
            sensorPulsePlot.InvalidatePlot(true);
        }
    }
}
