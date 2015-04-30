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

using OxyPlot;
using OxyPlot.Series;

namespace SensorPlots
{
    using Microsoft.Win32;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PlotModel plotModel = new PlotModel();

        private ISample[] data;

        private ISensorHub dataSource;

        private readonly LineSeries dataSeries = new LineSeries();

        public MainWindow()
        {
            InitializeComponent();
            plotModel.Series.Add(dataSeries);
            PlotView.Model = plotModel;
            Debug.WriteLine(unchecked((ushort)-2));
            dataSeries.StrokeThickness = 1;
        }

        private void BtnOpenFile(object sender, RoutedEventArgs e)
        {
            
            var dlg = new OpenFileDialog
            {
                Filter = "Binary files (*.bin)|*.bin"
            };
            if (dlg.ShowDialog(this) == true)
            {
                if (dataSource != null)
                {
                    dataSource.Dispose();
                    ChannelSelection.Items.Clear();
                    dataSource = null;
                }
                dataSource = new SensorDataFileStreamer(dlg.FileName);
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

            var startTime = data[0].Time;
            // For each sample:
            for (int i = 0; i < data.Length; ++i)
            {
                var x = (data[i].Time - startTime).TotalSeconds;
                var y = data[i].Values[sensorIdx].Value;
                dataSeries.Points.Add(new DataPoint(x, y));
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
