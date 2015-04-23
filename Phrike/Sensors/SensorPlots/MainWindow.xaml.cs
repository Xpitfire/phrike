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

        private short[] data;

        private ISensorDataSource dataSource;

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
                for (int i = 0; i < dataSource.AnalogChannels.Length; ++i)
                {
                    if (dataSource.AnalogChannels[i].HasValue)
                    {
                        ChannelSelection.Items.Add(i + 1);
                    }
                }
                data = dataSource.GetData(dataSource.GetAvailableDataCount());
            }
        }

        private void UpdatePlot()
        {
            var relativeChannelIdx = ChannelSelection.SelectedIndex;
            if (ChannelSelection.SelectedItem == null)
            {
                return;
            }
            var channelIdx = (int)ChannelSelection.SelectedItem - 1;
            var channel = dataSource.AnalogChannels[channelIdx].Value;
            var nChannels = dataSource.GetSampleShortCount();
            dataSeries.Points.Clear();
            dataSeries.Title = "Channel " + ChannelSelection.SelectedItem;

            // As per gMOBIlabplusDataFormatv209a.pdf page 3
            var scale = (2 * 5 * 1e-6 / (65536 * 4)) * channel.Sensitivity;

            // For each sample:
            for (int i = 0; i < data.Length / nChannels; ++i)
            {
                var y = data[(i * nChannels) + relativeChannelIdx] * scale;
                dataSeries.Points.Add(
                    new DataPoint(i / channel.SampleRate, y));
            }
            PlotView.InvalidatePlot(true);
        }

        private void CbChannelSelected(object sender, SelectionChangedEventArgs e)
        {
            UpdatePlot();
        }
    }
}
