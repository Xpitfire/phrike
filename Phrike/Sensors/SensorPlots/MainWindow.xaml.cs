// <summary>Implementation of the main window logic.</summary>
// -----------------------------------------------------------------------
// Copyright (c) 2015 University of Applied Sciences Upper-Austria
// Project OperationPhrike
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR 
// ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION 
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
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
        /// <summary>
        /// The OxyPlot plotmodel that is displayed in the window.
        /// </summary>
        private readonly PlotModel plotModel = new PlotModel();

        /// <summary>
        /// The prefiltered data series that is displayed in the PlotModel.
        /// </summary>
        private readonly LineSeries dataSeries = new LineSeries();

        /// <summary>
        /// Series of minimum peaks.
        /// </summary>
        private readonly LineSeries minSeries = new LineSeries();

        /// <summary>
        /// Series of maximum peaks.
        /// </summary>
        private readonly LineSeries maxSeries = new LineSeries();

        /// <summary>
        /// Series of merged (minimum + maximum) peaks.
        /// </summary>
        private readonly LineSeries mergedPeaksSeries = new LineSeries();

        /// <summary>
        /// Buffer that contains the samples read from <see cref="dataSource"/>.
        /// </summary>
        private ISample[] data;

        /// <summary>
        /// The sensor hub corresponding to <see cref="data"/>.
        /// </summary>
        private ISensorHub dataSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.plotModel.Series.Add(this.dataSeries);
            this.plotModel.Series.Add(this.minSeries);
            this.plotModel.Series.Add(this.maxSeries);
            this.plotModel.Series.Add(this.mergedPeaksSeries);
            this.PlotView.Model = this.plotModel;
            this.dataSeries.StrokeThickness = 1;
            this.minSeries.StrokeThickness = 1;
            this.maxSeries.StrokeThickness = 1;
            this.mergedPeaksSeries.StrokeThickness = 1;
        }

        /// <summary>
        /// Invoked when the "Open file" button is clicked.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Additional event arguments.</param>
        private void BtnOpenFileClicked(object sender, RoutedEventArgs e)
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

        /// <summary>
        /// Update the plot from <see cref="data"/> and the selected Dropdown item.
        /// </summary>
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
            
            double[] sensorData = SensorUtil.GetSampleValues(this.data, sensorIdx).ToArray();

            var filterChain = new FilterChain();

            filterChain.Add(new GaussFilter(4));
            filterChain.Add(new EdgeDetectionFilter(2));
            IReadOnlyList<double> prefilteredData = filterChain.Filter(sensorData);
            IReadOnlyList<double> maxPeaks = new PeakFilter(15).Filter(prefilteredData);
            var maxFilter = new BinaryThresholdFilter(0.5);
            maxPeaks = maxFilter.Filter(maxPeaks);
            IReadOnlyList<double> minPeaks = new PeakFilter(15, false).Filter(prefilteredData);
            minPeaks = new BinaryThresholdFilter(0.5, false).Filter(minPeaks);
            IReadOnlyList<double> mergedPeaks = HeartPeakFilter.MergePeaks(
                maxPeaks, minPeaks, 11);
            mergedPeaks = maxFilter.Filter(mergedPeaks);

            foreach (var pulse in new PulseCalculator().Filter(mergedPeaks))
            {
                Debug.WriteLine(pulse);
            }
           
            var startTime = this.data[0].Time;
            for (int i = 0; i < sensorData.Length; ++i)
            {
                var x = (this.data[i].Time - startTime).TotalSeconds;
                this.dataSeries.Points.Add(new DataPoint(x, prefilteredData[i]));
                this.minSeries.Points.Add(new DataPoint(x, minPeaks[i]));
                this.maxSeries.Points.Add(new DataPoint(x, maxPeaks[i]));
                this.mergedPeaksSeries.Points.Add(new DataPoint(x, mergedPeaks[i]));
            }

            this.PlotView.InvalidatePlot(true);
        }

        /// <summary>
        /// Invoked when the selection in the channel selection Dropdown changes.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Additional event arguments.</param>
        private void CbChannelSelected(object sender, SelectionChangedEventArgs e)
        {
            this.UpdatePlot();
        }
    }
}
