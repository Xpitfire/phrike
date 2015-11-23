using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;

using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

using Phrike.GroundControl.Annotations;
using Phrike.Sensors;


namespace Phrike.GroundControl.ViewModels
{
    public class DataSeriesViewModel : INotifyPropertyChanged
    {
        public DataSeries Model { get; }

        public LinearAxis YAxis { get; }

        public bool IsActive
        {
            get { return isActive; }
            set
            {
                if (value == isActive)
                {
                    return;
                }
                isActive = value;
                OnPropertyChanged();
            }
        }

        private LineSeries plottableData;

        private bool isActive;

        private Brush color;

        private bool isTrendShown;

        private LineSeries trendSeries;

        public DataSeriesViewModel(DataSeries model)
        {
            Model = model;
            YAxis = new LinearAxis { Key = model.FullName };
        }

        public Series PlottableData
        {
            get
            {
                if (plottableData == null)
                {
                    plottableData = new LineSeries { YAxisKey = YAxis.Key };
                    plottableData.Points.AddRange(Model.Data.Select(
                        (y, i) => new DataPoint(i / (double)Model.SampleRate, y)));
                }

                return plottableData;
            }
        }

        public string Name => Model.Name;

        public Statistics Statistics => Model.Statistics;

        public Brush Color => color ?? (color = new SolidColorBrush(
            OxyPlot.Wpf.ConverterExtensions.ToColor(plottableData.ActualColor))
        );

        public double Interval => (Model.Data.Length - 1) / (double)Model.SampleRate;

        public bool IsTrendShown
        {
            get { return isTrendShown; }
            set
            {
                if (value == isTrendShown)
                    return;
                isTrendShown = value;
                OnPropertyChanged();
            }
        }

        public Series TrendSeries
        {
            get
            {
                if (trendSeries == null)
                {
                    trendSeries = new LineSeries
                    {
                        YAxisKey = YAxis.Key,
                        Color = plottableData.ActualColor,
                        LineStyle = LineStyle.Dash
                    };
                    trendSeries.Points.Add(new DataPoint(0, Statistics.Intercept));
                    trendSeries.Points.Add(new DataPoint(
                        Interval,
                        Statistics.Intercept + Model.Data.Length * Statistics.Slope));
                }
                return trendSeries;
            }
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