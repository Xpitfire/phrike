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
    /// <summary>
    /// View model for a <see cref="DataSeries"/>.
    /// </summary>
    public class DataSeriesViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the model of this view.
        /// </summary>
        public DataSeries Model { get; }

        /// <summary>
        /// Gets the Y axis of this view.
        /// </summary>
        public LinearAxis YAxis { get; }

        /// <summary>
        /// Gets or sets whether this view model should be displayed.
        /// </summary>
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

        /// <summary>
        /// Backing field for <see cref="plottableData"/>.
        /// </summary>
        private LineSeries plottableData;

        /// <summary>
        /// Backing field for <see cref="IsActive"/>.
        /// </summary>
        private bool isActive;

        /// <summary>
        /// Backing field for <see cref="Color"/>.
        /// </summary>
        private Brush color;

        /// <summary>
        /// Backing field for <see cref="IsTrendShown"/>.
        /// </summary>
        private bool isTrendShown;

        /// <summary>
        /// Backing field for <see cref="TrendSeries"/>.
        /// </summary>
        private LineSeries trendSeries;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSeriesViewModel"/> class.
        /// </summary>
        /// <param name="model">Value for <see cref="Model"/>.</param>
        public DataSeriesViewModel(DataSeries model)
        {
            Model = model;
            YAxis = new LinearAxis { Key = model.FullName };
        }

        /// <summary>
        /// Gets a OxyPlot <see cref="Series"/> to display in a <see cref="PlotModel"/>.
        /// The <see cref="YAxis"/> must be added first!
        /// </summary>
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

        /// <summary>
        /// See <see cref="DataSeries.Name"/>.
        /// </summary>
        public string Name => Model.Name;

        /// <summary>
        /// See <see cref="DataSeries.Statistics"/>.
        /// </summary>
        public Statistics Statistics => Model.Statistics;

        /// <summary>
        /// Gets the actual color of <see cref="PlottableData"/>.
        /// </summary>
        public Brush Color => color ?? (color = new SolidColorBrush(
            OxyPlot.Wpf.ConverterExtensions.ToColor(plottableData.ActualColor))
        );

        /// <summary>
        /// The length of the time interval contained in the <see cref="Model"/> in seconds.
        /// </summary>
        public double Interval => (Model.Data.Length - 1) / (double)Model.SampleRate;

        /// <summary>
        /// Gets or sets whether to show a trend line (linear regression) or not.
        /// </summary>
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

        /// <summary>
        /// Gets an OxyPlot <see cref="Series"/> to display as a trend line.
        /// </summary>
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