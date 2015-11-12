using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using OxyPlot;
using OxyPlot.Series;

using Phrike.GroundControl.Annotations;
using Phrike.Sensors;

namespace Phrike.GroundControl.ViewModels
{
    public class DataSeriesViewModel : INotifyPropertyChanged
    {
        public DataSeries Model { get; }

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

        public DataSeriesViewModel(DataSeries model)
        {
            Model = model;
        }

        public Series PlottableData
        {
            get
            {
                if (plottableData == null)
                {
                    plottableData = new LineSeries();
                    plottableData.Points.AddRange(Model.Data.Select(
                        (y, i) => new DataPoint(i / (double)Model.SampleRate, y)));
                }

                return plottableData;
            }
        }

        public string Name => Model.Name;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}