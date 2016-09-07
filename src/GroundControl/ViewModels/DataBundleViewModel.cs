using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using OxyPlot;
using OxyPlot.Axes;

using Phrike.GroundControl.Annotations;
using Phrike.Sensors;

namespace Phrike.GroundControl.ViewModels
{
    /// <summary>
    /// View model for <see cref="DataBundle"/>.
    /// </summary>
    public class DataBundleViewModel: INotifyPropertyChanged
    {
        /// <summary>
        /// The X Axis of the plot (time in seconds).
        /// </summary>
        private readonly LinearAxis xAxis = new LinearAxis {
            AbsoluteMinimum = 0,
            Position = AxisPosition.Bottom
        };

        /// <summary>
        /// Backing field for <see cref="ResetView"/>.
        /// </summary>
        private ICommand resetView;

        /// <summary>
        /// Backing field for <see cref="RightAxis"/>.
        /// </summary>
        private DataSeriesViewModel rightAxis;

        /// <summary>
        /// Backing field for <see cref="LeftAxis"/>.
        /// </summary>
        private DataSeriesViewModel leftAxis;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataBundleViewModel"/> class.
        /// </summary>
        /// <param name="model">The model of this view.</param>
        public DataBundleViewModel(DataBundle model)
        {
            DataSeries = new ObservableCollection<DataSeriesViewModel>(
                model.DataSeries.Select((ds, i) => new DataSeriesViewModel(ds, i) {IsActive = true}));

            Initialize();
        }

        private void Initialize()
        {

            PlotModel = new PlotModel();
            PlotModel.Axes.Add(xAxis);
            xAxis.TransformChanged +=
                (s, e) =>
                {
                    OnPropertyChanged(nameof(CurrentlyShownInterval));
                    OnPropertyChanged(nameof(MaximumStartPosition));
                    OnPropertyChanged(nameof(StartPosition));
                };

            if (DataSeries.Count > 0)
            {
                foreach (DataSeriesViewModel ds in DataSeries)
                {
                    ds.PropertyChanged += DataSeriesPropertyChanged;
                    if (ds.IsActive)
                        AddActiveSeries(ds);
                }
                xAxis.AbsoluteMaximum = DataSeries.Select(s => s.Interval).Max();
                LeftAxis = DataSeries.FirstOrDefault(ds => ds.IsActive);
                RightAxis = DataSeries.Where(ds => ds.IsActive).Skip(1).FirstOrDefault();
            }
        }

        /// <summary>
        ///  The bools indicate whether to show by default.
        /// </summary>
        public DataBundleViewModel(IEnumerable<KeyValuePair<DataSeries, bool>> bundle)
        {
            DataSeries = new ObservableCollection<DataSeriesViewModel>();
            var idx = 0;
            foreach (KeyValuePair<DataSeries, bool> ds in bundle)
            {
                DataSeries.Add(new DataSeriesViewModel(ds.Key, idx) { IsActive = ds.Value });
                ++idx;
            }
            Initialize();
        }

        /// <summary>
        /// Handles changes of any contained <see cref="DataSeriesViewModel"/>s.
        /// </summary>
        private void DataSeriesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var ds = (DataSeriesViewModel)sender;
            if (e.PropertyName == nameof(DataSeriesViewModel.IsActive))
            {
                if (ds.IsActive)
                {
                    AddActiveSeries(ds);
                }
                else
                {
                    PlotModel.Series.Remove(ds.PlottableData);
                    ds.IsTrendShown = false;
                    PlotModel.Axes.Remove(ds.YAxis);
                    ActiveSeries.Remove(ds);
                }
            } else if (e.PropertyName == nameof(DataSeriesViewModel.IsTrendShown))
            {
                if (ds.IsTrendShown)
                {
                    ds.IsActive = true;
                    PlotModel.Series.Add(ds.TrendSeries);
                }
                else
                {
                    PlotModel.Series.Remove(ds.TrendSeries);
                }
            }
            PlotModel.InvalidatePlot(false);
        }

        private void AddActiveSeries(DataSeriesViewModel ds)
        {
            PlotModel.Axes.Add(ds.YAxis);
            PlotModel.Series.Add(ds.PlottableData);
            ActiveSeries.Add(ds);
        }

        /// <summary>
        /// Gets the OxyPlot <see cref="PlotModel"/> in which all active series and their trends
        /// (if active) are displayed.
        /// </summary>
        public PlotModel PlotModel { get; private set; }

        /// <summary>
        /// Gets the list of all contained data series. Do not modify!
        /// </summary>
        public ObservableCollection<DataSeriesViewModel> DataSeries { get; }

        /// <summary>
        /// Gets or sets the Y axis displayed on the left side of the plot.
        /// </summary>
        public DataSeriesViewModel LeftAxis
        {
            get { return leftAxis; }
            set
            {
                if (Equals(value, leftAxis))
                {
                    return;
                }
                if (value != null && Equals(value, RightAxis))
                {
                    rightAxis = leftAxis;
                    OnPropertyChanged(nameof(RightAxis));
                }
                leftAxis = value;
                UpdateAxes();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Y axis displayed on the right side of the plot.
        /// </summary>
        public DataSeriesViewModel RightAxis
        {
            get { return rightAxis; }
            set
            {
                if (Equals(value, rightAxis))
                {
                    return;
                }
                if (value != null && Equals(value, LeftAxis))
                {
                    leftAxis = rightAxis;
                    OnPropertyChanged(nameof(LeftAxis));
                }
                rightAxis = value;
                UpdateAxes();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Updates the plot's axes in response to changes of
        /// <see cref="LeftAxis"/> or <see cref="RightAxis"/>.
        /// </summary>
        private void UpdateAxes()
        {
            foreach (LinearAxis axis in ActiveSeries.Select(ds => ds.YAxis))
            {
                if (LeftAxis != null && axis == LeftAxis.YAxis)
                {
                    axis.IsAxisVisible = true;
                    axis.Position = AxisPosition.Left;
                }
                else if (RightAxis != null && axis == RightAxis.YAxis)
                {
                    axis.IsAxisVisible = true;
                    axis.Position = AxisPosition.Right;
                }
                else
                {
                    axis.IsAxisVisible = false;
                }
            }
            PlotModel.InvalidatePlot(false);
        }


        /// <summary>
        /// Gets the time of the last datapoint == the length of the longest
        /// <see cref="DataSeries"/> in seconds.
        /// </summary>
        public double TotalInterval => DataSeries
            .Select(s => s.Interval).DefaultIfEmpty().Max();

        /// <summary>
        /// Gets the maximal minimum value of the X axis. Determined by
        /// <see cref="TotalInterval"/> and by how big the interval currently
        /// shown is.
        /// </summary>
        public double MaximumStartPosition => TotalInterval - CurrentlyShownInterval;

        /// <summary>
        /// Gets the length of the interval that is currently visible on the plot (in seconds).
        /// </summary>
        private double CurrentlyShownInterval => xAxis.ActualMaximum - xAxis.ActualMinimum;

        /// <summary>
        /// Gets or sets the minimum time for datapoints to be visible on the plot (in seconds).
        /// </summary>
        public double StartPosition
        {
            get { return xAxis.ActualMinimum; }
            set
            {
                xAxis.Zoom(value, value + CurrentlyShownInterval);
                PlotModel.InvalidatePlot(true);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the time (in seconds) which data points in the X center of the plot have.
        /// </summary>
        public double CenterPosition
        {
            get { return StartPosition + (CurrentlyShownInterval / 2); }
            set { StartPosition = value - CurrentlyShownInterval / 2; }
        }

        /// <summary>
        /// Gets a command that resets pan & zoom of the plot when executed.
        /// </summary>
        public ICommand ResetView
            => resetView ?? (resetView = new RelayCommand(DoResetView));

        /// <summary>
        /// Gets all currently active series. Do not modify!
        /// </summary>
        public ObservableCollection<DataSeriesViewModel> ActiveSeries { get; }
            = new ObservableCollection<DataSeriesViewModel>();

        /// <summary>
        /// Command execution handler for <see cref="ResetView"/>.
        /// </summary>
        private void DoResetView(object obj)
        {
            PlotModel.ResetAllAxes();
            PlotModel.InvalidatePlot(true);
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Fires <see cref="PropertyChanged"/>.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}

