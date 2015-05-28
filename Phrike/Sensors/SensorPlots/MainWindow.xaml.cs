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
        /// The data series that is displayed in the PlotModel.
        /// </summary>
        private readonly ScatterSeries dataSeries = new ScatterSeries();

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
            this.PlotView.Model = this.plotModel;
            ////dataSeries.StrokeThickness = 1;
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
