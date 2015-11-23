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
    public class DataBundleViewModel: INotifyPropertyChanged
    {
        private readonly LinearAxis xAxis = new LinearAxis {
            AbsoluteMinimum = 0,
            Position = AxisPosition.Bottom
        };

        private ICommand resetView;

        private DataSeriesViewModel rightAxis;

        private DataSeriesViewModel leftAxis;

        public DataBundleViewModel(DataBundle model)
        {
            DataSeries = new ObservableCollection<DataSeriesViewModel>(
                model.DataSeries.Select(ds => new DataSeriesViewModel(ds)));

            foreach (DataSeriesViewModel ds in DataSeries)
            {
                ds.PropertyChanged += DataSeriesPropertyChanged;
            }

            PlotModel = new PlotModel();
            PlotModel.Axes.Add(xAxis);
            xAxis.TransformChanged +=
                (s, e) =>
                {
                    OnPropertyChanged(nameof(CurrentlyShownInterval));
                    OnPropertyChanged(nameof(MaximumStartPosition));
                    OnPropertyChanged(nameof(StartPosition));
                };

            foreach (DataSeriesViewModel series in DataSeries)
            {
                series.IsActive = true;
            }

            xAxis.AbsoluteMaximum = DataSeries.Select(s => s.Interval).Max();
            if (DataSeries.Count > 0)
            {
                LeftAxis = DataSeries.First();
                if (DataSeries.Count > 1)
                {
                    RightAxis = DataSeries[1];
                }
            }
        }

        private void DataSeriesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var ds = (DataSeriesViewModel)sender;
            if (e.PropertyName == nameof(DataSeriesViewModel.IsActive))
            {
                if (ds.IsActive)
                {
                    PlotModel.Axes.Add(ds.YAxis);
                    PlotModel.Series.Add(ds.PlottableData);
                    ActiveSeries.Add(ds);
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

        public PlotModel PlotModel { get; }

        public ObservableCollection<DataSeriesViewModel> DataSeries { get; }

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


        public double TotalInterval => DataSeries
            .Select(s => s.Interval).Max();

        /// <summary>
        /// The maximal minimum value of the X axis. Determined by
        /// <see cref="TotalInterval"/> and by how big the interval currently
        /// shown is.
        /// </summary>
        public double MaximumStartPosition => TotalInterval - CurrentlyShownInterval;

        private double CurrentlyShownInterval => xAxis.ActualMaximum - xAxis.ActualMinimum;

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

        public ICommand ResetView
            => resetView ?? (resetView = new RelayCommand(DoResetView));

        public ObservableCollection<DataSeriesViewModel> ActiveSeries { get; }
            = new ObservableCollection<DataSeriesViewModel>();

        private void DoResetView(object obj)
        {
            PlotModel.ResetAllAxes();
            PlotModel.InvalidatePlot(true);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}

