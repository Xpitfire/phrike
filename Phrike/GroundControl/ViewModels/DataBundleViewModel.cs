using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

using OxyPlot;

using Phrike.Sensors;

namespace Phrike.GroundControl.ViewModels
{
    public class DataBundleViewModel
    {
        public DataBundleViewModel(DataBundle model)
        {
            DataSeries = new ObservableCollection<DataSeriesViewModel>(
                model.DataSeries.Select(ds => new DataSeriesViewModel(ds)));
            foreach (DataSeriesViewModel ds in DataSeries)
            {
                ds.PropertyChanged += DataSeriesPropertyChanged;
            }
            PlotModel = new PlotModel();

            foreach (DataSeriesViewModel series in DataSeries)
            {
                series.IsActive = true;
            }
        }

        private void DataSeriesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(DataSeriesViewModel.IsActive))
            {
                return;
            }

            var ds = (DataSeriesViewModel)sender;
            if (ds.IsActive)
            {
                PlotModel.Series.Add(ds.PlottableData);
            }
            else
            {
                PlotModel.Series.Remove(ds.PlottableData);
            }
            PlotModel.InvalidatePlot(false);
        }

        public PlotModel PlotModel { get; }

        public ObservableCollection<DataSeriesViewModel> DataSeries { get; }
    }
}

