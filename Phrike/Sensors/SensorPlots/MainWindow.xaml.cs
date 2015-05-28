using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Win32;

using OperationPhrike.GMobiLab;
using OperationPhrike.Sensors;
using OperationPhrike.Sensors.Filters;

using OxyPlot;
using OxyPlot.Series;

namespace OperationPhrike.SensorPlots
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PlotModel plotModel = new PlotModel();

        private readonly ScatterSeries dataSeries = new ScatterSeries();

        private ISample[] data;

        private ISensorHub dataSource;

        public MainWindow()
        {
            this.InitializeComponent();
            this.plotModel.Series.Add(this.dataSeries);
            this.PlotView.Model = this.plotModel;
            ////dataSeries.StrokeThickness = 1;
        }

        private void BtnOpenFile(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Binary files (*.bin)|*.bin"
            };

            // ShowDialog returns bool?, hence ==.
            if (dlg.ShowDialog() == true) 
            {
                if (this.dataSource != null)
                {
                    this.dataSource.Dispose();
                    this.ChannelSelection.Items.Clear();
                    this.dataSource = null;
                }

                this.dataSource = new SensorDataFileStreamer(dlg.FileName);
                foreach (var sensor in this.dataSource.Sensors)
                {
                    if (sensor.Enabled)
                    {
                        this.ChannelSelection.Items.Add(sensor);
                    }
                }

                this.data = this.dataSource.ReadSamples().ToArray();
            }
        }

        private void UpdatePlot()
        {
            if (this.ChannelSelection.SelectedItem == null || this.data.Length <= 0)
            {
                return;
            }

            var sensor = (SensorInfo)this.ChannelSelection.SelectedItem;
            this.dataSeries.Points.Clear();
            this.dataSeries.Title = sensor.Name;

            int sensorIdx = this.dataSource.GetSensorValueIndexInSample(sensor);
            
            double[] sensorData = SensorUtil.GetSampleValues(this.data, sensorIdx);

            var filterChain = new FilterChain();

            filterChain.Add(new GaussFilter(4));
            filterChain.Add(new PeakFilter(0.0009, 100));
            filterChain.Add(new ValueDistanceFilter());
            filterChain.Add(new GaussFilter(3));

            sensorData = filterChain.Filter(sensorData)
                .Select(v => 1 / v * 256 * 60)
                .ToArray();

            var startTime = this.data[0].Time;
            for (int i = 0; i < sensorData.Length; ++i)
            {
                var x = (this.data[i].Time - startTime).TotalSeconds;
                var y = sensorData[i];
                this.dataSeries.Points.Add(new ScatterPoint(x, y));
            }

            this.PlotView.InvalidatePlot(true);
        }

        private void CbChannelSelected(object sender, SelectionChangedEventArgs e)
        {
            this.UpdatePlot();
        }
    }
}
