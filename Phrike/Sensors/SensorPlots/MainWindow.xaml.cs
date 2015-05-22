using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using OperationPhrike.GMobiLab;
using OperationPhrike.Sensors;

using OperationPhrike.SensorFilters;
using OxyPlot;
using OxyPlot.Series;

namespace SensorPlots
{
    using Microsoft.Win32;

    using OperationPhrike.Sensors.Filters;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PlotModel plotModel = new PlotModel();

        private ISample[] data;

        private ISensorHub dataSource;

        private readonly ScatterSeries dataSeries = new ScatterSeries();

        public MainWindow()
        {
            InitializeComponent();
            plotModel.Series.Add(dataSeries);
            PlotView.Model = plotModel;
            //dataSeries.StrokeThickness = 1;

            var data = new double[]
            {
                200, 4, 2, 3, 6, 20, 100, 15, 5, 3, -1, -5, 0, 0, 30, 150, 50, 20, 1, 5, 4, 1, 0, 1, 60
                //0, 1, 2, 3, 4, 5, 6, 7, 8
            };

            FilterBase filter = new ExtremeChangeFilter(200, 50);

            IReadOnlyList<double> filterData = filter.Filter(data);
            const int SampleRate = 256;
            filterData = filter.Filter(data);


            foreach (double d in filterData)
            {
                Console.Write(d + " ");
            }


        }

        private void BtnOpenFile(object sender, RoutedEventArgs e)
        {
            
            var dlg = new OpenFileDialog
            {
                Filter = "Binary files (*.bin)|*.bin"
            };
            if (true)
            {
                if (dataSource != null)
                {
                    dataSource.Dispose();
                    ChannelSelection.Items.Clear();
                    dataSource = null;
                }
                dataSource = new SensorDataFileStreamer(@"\\FSHOME\homes\se\s1310307058\Profile\Desktop\Studium\EKG-Signal_zur_Verarbeitung_23_04_15.bin");
                foreach (var sensor in dataSource.Sensors)
                {
                    if (sensor.Enabled)
                    {
                        ChannelSelection.Items.Add(sensor);
                    }
                }
                data = dataSource.ReadSamples().ToArray();
            }
        }

        private void UpdatePlot()
        {
            var relativeChannelIdx = ChannelSelection.SelectedIndex;
            if (ChannelSelection.SelectedItem == null || data.Length <= 0)
            {
                return;
            }

            var sensor = (SensorInfo)ChannelSelection.SelectedItem;
            dataSeries.Points.Clear();
            dataSeries.Title = sensor.Name;

            int sensorIdx = dataSource.GetSensorValueIndexInSample(sensor);
            
            double[] sensorData = SensorUtil.GetSampleValues(data, sensorIdx);
            sensorData =
                new ExtremeChangeFilter(200, 10).Filter(
                //new GaussFilter(3).Filter(
                    //new MedianFilter(8).Filter(
                        new ValueDistanceFilter().Filter(
                            new PeakFilter(0.0009, 100).Filter(sensorData)
                                //new GaussFilter(4).Filter(sensorData)
                            //)
                        //)
                    //)
                )
                )
                .Select(v => 1 / v * 256 * 60)
                .ToArray(); 
            
         

            var startTime = data[0].Time;
            // For each sample:
            for (int i = 0; i < sensorData.Length; ++i)
            {
                var x = (data[i].Time - startTime).TotalSeconds;
                var y = sensorData[i];
                dataSeries.Points.Add(new ScatterPoint(x, y));
                //Debug.WriteLine(y);
            }
            PlotView.InvalidatePlot(true);
        }

        private void CbChannelSelected(object sender, SelectionChangedEventArgs e)
        {
            UpdatePlot();
        }
    }
}
