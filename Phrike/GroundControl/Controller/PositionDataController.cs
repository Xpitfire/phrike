using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Axes;
using DataModel;
using OxyPlot.Series;
using DataAccess;

namespace Phrike.GroundControl.Controller
{

    class PositionDataController
    {
        public PlotModel PositionSpeedPlot { get; private set; }
        public PlotModel PositionAccelPlot { get; private set; }
        public PlotModel PositionIdleMovementPlot { get; private set; }
        public Double AverageSpeed { get; private set; }
        public Double AverageAccel { get; private set; }
        public Double TotalDistance { get; private set; }
        public Double Altitude { get; private set; }
        public Double MaxSpeed { get; private set; }

        public TimeSpan TotalTime { get; private set; }
        public TimeSpan TotalIdleTime { get; private set; }

        public PositionDataController()
        {
            PositionSpeedPlot = CreateSpeedPlotModel();
            PositionAccelPlot = CreateAccelPlotModel();
            PositionIdleMovementPlot = CreateIdleMovementPlotModel();
        }

        public PlotModel CreateSpeedPlotModel()
        {
            PlotModel positionSpeedPlot = new PlotModel { Title = "Movement Speed Results", PlotAreaBackground = OxyColors.Transparent };
            positionSpeedPlot.LegendBorder = OxyColors.Transparent;

            var yAxis = new LinearAxis();
            yAxis.Position = AxisPosition.Left;
            yAxis.Minimum = 0;
            yAxis.Title = "Speed (km/h)";
            yAxis.MinorStep = 5;
            yAxis.MajorStep = 20;
            yAxis.MajorGridlineStyle = LineStyle.Solid;
            yAxis.MajorGridlineColor = OxyColors.LightGray;
            yAxis.MinorGridlineStyle = LineStyle.Solid;
            yAxis.MinorGridlineColor = OxyColor.FromRgb(240, 240, 240);

            var xAxis = new LinearAxis();
            xAxis.Position = AxisPosition.Bottom;
            xAxis.Title = "Time (sec)";

            positionSpeedPlot.Axes.Add(yAxis);
            positionSpeedPlot.Axes.Add(xAxis);

            return positionSpeedPlot;
        }

        public PlotModel CreateAccelPlotModel()
        {
            PlotModel positionAccelPlot = new PlotModel { Title = "Acceleration Results", PlotAreaBackground = OxyColors.Transparent };
            positionAccelPlot.LegendBorder = OxyColors.Transparent;

            var yAxis = new LinearAxis();
            yAxis.Position = AxisPosition.Left;
            yAxis.Title = "Accel (m/s^2)";
            yAxis.MinorStep = 5;
            yAxis.MajorStep = 20;
            yAxis.MajorGridlineStyle = LineStyle.Solid;
            yAxis.MajorGridlineColor = OxyColors.LightGray;
            yAxis.MinorGridlineStyle = LineStyle.Solid;
            yAxis.MinorGridlineColor = OxyColor.FromRgb(240, 240, 240);

            var xAxis = new LinearAxis();
            xAxis.Position = AxisPosition.Bottom;
            xAxis.Title = "Time (sec)";

            positionAccelPlot.Axes.Add(yAxis);
            positionAccelPlot.Axes.Add(xAxis);

            return positionAccelPlot;
        }

        public PlotModel CreateIdleMovementPlotModel()
        {
            PlotModel positionIdleMovementPlot = new PlotModel { Title = "Idle Movement Results", PlotAreaBackground = OxyColors.Transparent };
            positionIdleMovementPlot.LegendBorder = OxyColors.Transparent;

            var yAxis = new LinearAxis();
            yAxis.Position = AxisPosition.Left;
            yAxis.Minimum = 0;
            yAxis.Title = "Idle Time";
            yAxis.MajorGridlineStyle = LineStyle.Solid;
            yAxis.MajorGridlineColor = OxyColors.LightGray;
            yAxis.MinorGridlineStyle = LineStyle.Solid;
            yAxis.MinorGridlineColor = OxyColor.FromRgb(240, 240, 240);

            var xAxis = new LinearAxis();
            xAxis.Position = AxisPosition.Bottom;
            xAxis.Title = "Time (sec)";

            positionIdleMovementPlot.Axes.Add(yAxis);
            positionIdleMovementPlot.Axes.Add(xAxis);

            return positionIdleMovementPlot;
        }

        private void CalculatePlotData(IEnumerable<PositionData> allPositionData)
        {
            const Double CM_TO_KM = 100000;
            const Double CM_TO_M = 100;
            const Double MS_TO_S = 1000;
            const Double H_TO_MS = MS_TO_S * 3600;
            const Double CMPMS2_TO_MPS2 = MS_TO_S * MS_TO_S / CM_TO_M;
            const Double CMPMS_TO_KMPH = H_TO_MS / CM_TO_KM;

            var speedLineSeries = new LineSeries();
            var accelLineSeries = new LineSeries();
            var idleMovementLineSeries = new LineSeries();
            Double totalDistance = 0;
            Double prevSpeed = 0;
            Double currentSpeed = 0;
            Double distance = 0;
            Double accel = 0;
            Double totalDistanceZ = 0;
            Double distanceZ = 0;
            int idleMovement = 0;
            DateTime startTime;
            TimeSpan timeElapsed = TimeSpan.Zero, totalTimeElapsed = TimeSpan.Zero;
            PositionData prevData, currentData;
            IEnumerator<PositionData> iter = allPositionData.GetEnumerator();
            iter.MoveNext();
            prevData = iter.Current;
            startTime = prevData.Time;
            TotalIdleTime = TimeSpan.Zero;
            while (iter.MoveNext())
            {
                currentData = iter.Current;
                timeElapsed = currentData.Time - prevData.Time;
                totalTimeElapsed = currentData.Time - startTime;
                distanceZ = Math.Abs(currentData.Z - prevData.Z);
                totalDistanceZ += distanceZ;
                distance = Math.Sqrt(Math.Pow(currentData.X - prevData.X, 2) + Math.Pow(currentData.Y - prevData.Y, 2) + Math.Pow(distanceZ, 2));
                totalDistance += distance;
                currentSpeed = distance / timeElapsed.TotalMilliseconds;
                accel = (currentSpeed - prevSpeed) / (timeElapsed.TotalMilliseconds);
                speedLineSeries.Points.Add(new DataPoint(totalTimeElapsed.TotalSeconds, currentSpeed * CMPMS_TO_KMPH));
                accelLineSeries.Points.Add(new DataPoint(totalTimeElapsed.TotalSeconds, accel * CMPMS2_TO_MPS2));
                if (currentSpeed == 0)
                {
                    idleMovement++;
                    TotalIdleTime += timeElapsed;
                }
                else
                {
                    idleMovement = 0;
                }
                idleMovementLineSeries.Points.Add(new DataPoint(totalTimeElapsed.TotalSeconds, idleMovement));
                prevData = currentData;
                prevSpeed = currentSpeed;
            }

            AverageSpeed = speedLineSeries.Points.Average(p => p.Y);
            AverageAccel = accelLineSeries.Points.Average(p => p.Y);
            MaxSpeed = speedLineSeries.Points.Max(p => p.Y);
            TotalTime = totalTimeElapsed;
            TotalDistance = totalDistance / CM_TO_KM;
            Altitude = totalDistanceZ / CM_TO_KM;
            speedLineSeries.Color = OxyColors.Blue;
            speedLineSeries.StrokeThickness = 2;
            speedLineSeries.Smooth = true;
            PositionSpeedPlot.Series.Add(speedLineSeries);
            PositionSpeedPlot.InvalidatePlot(true);

            accelLineSeries.Color = OxyColors.Blue;
            accelLineSeries.StrokeThickness = 2;
            accelLineSeries.Smooth = true;
            PositionAccelPlot.Series.Add(accelLineSeries);
            PositionAccelPlot.InvalidatePlot(true);

            idleMovementLineSeries.Color = OxyColors.Blue;
            idleMovementLineSeries.StrokeThickness = 2;
            PositionIdleMovementPlot.Series.Add(idleMovementLineSeries);
            PositionIdleMovementPlot.InvalidatePlot(true);
        }

        public void LoadPlotData(int id)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var allPositionData = unitOfWork.PositionDataRepository.Get(
                    data => data.Test.Id == id);
                CalculatePlotData(allPositionData);
            }
        }
        public void LoadPlotData(IEnumerable<PositionData> allPositionData)
        {
            CalculatePlotData(allPositionData);
        }
    }
}
