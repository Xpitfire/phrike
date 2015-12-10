using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using NLog;
using OxyPlot;
using Phrike.GroundControl.Controller;

namespace Phrike.GroundControl.ViewModels
{
    public class PositionDataViewModel : INotifyPropertyChanged
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly PositionDataController plotController;

        public event PropertyChangedEventHandler PropertyChanged;

        public PlotModel PositionSpeedPlot { get; private set; }
        public PlotModel PositionAccelPlot { get; private set; }
        public PlotModel PositionIdleMovementPlot { get; private set; }

        private Double averageSpeed;
        public Double AverageSpeed
        {
            get { return averageSpeed; }
            set { averageSpeed = value; NotifyPropertyChanged("AverageSpeed"); }
        }

        private Double maxSpeed;
        public Double MaxSpeed
        {
            get { return maxSpeed; }
            set { maxSpeed = value; NotifyPropertyChanged("MaxSpeed"); }
        }

        private Double totalDistance;
        public Double TotalDistance
        {
            get { return totalDistance; }
            set { totalDistance = value; NotifyPropertyChanged("TotalDistance"); }
        }

        private Double altitude;
        public Double Altitude
        {
            get { return altitude; }
            set { altitude = value; NotifyPropertyChanged("Altitude"); }
        }

        private TimeSpan totalTime;
        public TimeSpan TotalTime
        {
            get { return totalTime; }
            set { totalTime = value; NotifyPropertyChanged("TotalTime"); }
        }

        private TimeSpan totalIdleTime;
        public TimeSpan TotalIdleTime
        {
            get { return totalIdleTime; }
            set { totalIdleTime = value; NotifyPropertyChanged("TotalIdleTime"); }
        }


        /// <summary>
        /// Create a new analysis viemodel instance and add the default plot template.
        /// </summary>
        public PositionDataViewModel()
        {
            plotController = new PositionDataController();
            PositionSpeedPlot = plotController.PositionSpeedPlot;
            PositionAccelPlot = plotController.PositionAccelPlot;
            PositionIdleMovementPlot = plotController.PositionIdleMovementPlot;
            //TODO: Add user reference
            //LoadPositionData(1);
        }

        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Load pulse data from a selected file and add to the plot view.
        /// </summary>
        public async void LoadPositionData(int id)
        {
            try
            {
                await LoadPositionDataTask(id);
            }
            catch (Exception e)
            {
                //Logger.Error("Could not load pulse data!", e);
                Logger.Error(e, "Could not load position data!");
            }
        }

        public async Task LoadPositionDataTask(int id)
        {
            await Task.Run(() => plotController.LoadPlotData(id));
            Logger.Info("Position Data successfully loaded!");
            AverageSpeed = plotController.AverageSpeed;
            MaxSpeed = plotController.MaxSpeed;
            TotalDistance = plotController.TotalDistance;
            Altitude = plotController.Altitude;
            TotalTime = plotController.TotalTime;
            TotalIdleTime = plotController.TotalIdleTime;
        }
    }
}
