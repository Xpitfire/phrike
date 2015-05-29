using System;
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
        private readonly PlotModel plotModel = new PlotModel();

        private readonly LineSeries dataSeries = new LineSeries();
        private readonly LineSeries minSeries = new LineSeries();
        private readonly LineSeries maxSeries = new LineSeries();
        private readonly LineSeries mergedPeaksSeries = new LineSeries();

        private ISample[] data;

        private ISensorHub dataSource;

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

        private IReadOnlyList<double> MergePeaks(IReadOnlyList<double> maxPeaks, IReadOnlyList<double> minPeaks)
        {
            const int MaxSearchDistance = 11;
            double[] result = new double[maxPeaks.Count];
            for (int i = 0; i < maxPeaks.Count; ++i)
            {
                if (maxPeaks[i] != 0)
                {
                    bool found = false;
                    for (int j = i; j < i + MaxSearchDistance && j < maxPeaks.Count && !found; ++j)
                    {
                        if (minPeaks[j] != 0)
                        {
                            result[i] = maxPeaks[i] - minPeaks[j];
                            found = true;
                        }
                    }
                }
            }
            return result;
        }

        private IReadOnlyList<double> BinaryThreshold(IReadOnlyList<double> peaks, bool detectMaxima, double thresholdRatio)
        {
            // median of top n / 500
            // Calculate reference height
            int targetNumber = peaks.Count / 500;
            double referenceValue = peaks
                .OrderByDescending(v => detectMaxima ? v : -v)
                .Skip(targetNumber / 2)
                .First();
            double cutoffMin = referenceValue * thresholdRatio;
            double[] result = new double[peaks.Count];

            for (int i = 0; i < peaks.Count; ++i)
            {
                if ((peaks[i] > cutoffMin && detectMaxima)
                    || (peaks[i] < cutoffMin && !detectMaxima))
                {
                    result[i] = peaks[i];
                }
            }
            return result;
        }
        private IReadOnlyList<double> CalculatePulse(IReadOnlyList<double> peaks)
        {
            const int MinPulse = 30;
            const int MaxPulse = 250;

            int lastPeakPos = -1;
            double sampleDistanceInMs = 1000 / 256.0;
            var result = new List<double>();
            for (int i = 0; i < peaks.Count; i++)
            {
                if (peaks[i] > 0)
                {
                    if (lastPeakPos >= 0)
                    {
                        int distance = i - lastPeakPos;
                        double timeMs = distance * sampleDistanceInMs;
                        double pulse = (60 * 1000) / timeMs;
                        if (pulse >= MinPulse && pulse <= MaxPulse)
                        {
                            if (result.Count > 0)
                            {
                                double averagePulse = result.Skip(result.Count - 3).Average();
                                if (pulse > averagePulse * 1.2)
                                {
                                    Debug.WriteLine("Error: Discarded {0}, avg={1}", pulse, averagePulse);
                                }
                                else if (pulse < averagePulse * 0.8)
                                {
                                    double oneMissingPulse = pulse * 2;
                                    double twoMissingPulse = pulse * 3;
                                    double oneMissingToAvg = Math.Abs(averagePulse - oneMissingPulse);
                                    double twoMissingToAvg = Math.Abs(averagePulse - twoMissingPulse);
                                    double correctedPulse = oneMissingPulse;
                                    if (twoMissingToAvg < oneMissingToAvg)
                                    {
                                        correctedPulse = twoMissingPulse;
                                    }
                                    result.Add(correctedPulse);
                                    Debug.WriteLine(
                                        "Corrected {0} to {1}, avg={2}",
                                        pulse,
                                        correctedPulse,
                                        averagePulse);
                                }
                                else
                                {
                                    result.Add(pulse);
                                }
                            }
                            else
                            {
                                result.Add(pulse);
                            }
                        }
                    }

                    lastPeakPos = i;
                }
            }
            return result;
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
            
            double[] sensorData = SensorUtil.GetSampleValues(this.data, sensorIdx).ToArray();

            var filterChain = new FilterChain();

            filterChain.Add(new GaussFilter(4));
            filterChain.Add(new EdgeDetectionFilter(2));
            IReadOnlyList<double> prefilteredData = filterChain.Filter(sensorData);
            IReadOnlyList<double> maxPeaks = new PeakFilter(15).Filter(prefilteredData);
            maxPeaks = BinaryThreshold(maxPeaks, true, 0.5);
            IReadOnlyList<double> minPeaks = new PeakFilter(15, false).Filter(prefilteredData);
            minPeaks = BinaryThreshold(minPeaks, false, 0.5);
            IReadOnlyList<double> mergedPeaks = MergePeaks(maxPeaks, minPeaks);
            mergedPeaks = BinaryThreshold(mergedPeaks, true, 0.5);

            foreach (var pulse in CalculatePulse(mergedPeaks))
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

        private void CbChannelSelected(object sender, SelectionChangedEventArgs e)
        {
            this.UpdatePlot();
        }
    }
}
