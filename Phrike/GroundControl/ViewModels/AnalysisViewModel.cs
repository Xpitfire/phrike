// <summary></summary>
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

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using DataAccess;

using DataModel;

using NLog;

using Phrike.GroundControl.Annotations;
using Phrike.GroundControl.Controller;
using Phrike.GroundControl.Helper;
using Phrike.Sensors;
using Phrike.Sensors.Filters;

namespace Phrike.GroundControl.ViewModels
{
    public class AnalysisViewModel : INotifyPropertyChanged
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private const string PulseChannelName = "Channel 05";
        private const string SkinConductanceChannelName = "Channel 02"; // TODO Correct channel?

        private AuxiliaryDataListViewModel fileList;

        private DataBundleViewModel dataModel;

    private DataSeries positionDataMovement;

    private DataSeries positionDataAccel;

    private DataSeries positionDataIdle;

        /// <summary>
        ///     Create a new analysis viemodel instance and add the default plot template.
        /// </summary>
        public AnalysisViewModel(int testId)
        {
            if (!DataLoadHelper.IsLoadDataActive())
                return;
            LoadData(testId);
        }

        public DataBundleViewModel DataModel
        {
            get { return dataModel; }
            set
            {
                if (Equals(value, dataModel))
                {
                    return;
                }
                dataModel = value;
                OnPropertyChanged();
            }
        }

        public double TotalDistance { get; set; }

        public double Altitude { get; set; }

        public TimeSpan TotalTime { get; set; }

        public TimeSpan TotalIdleTime { get; set; }

        public AuxiliaryDataListViewModel FileList
        {
            get { return fileList; }
            set
            {
                if (Equals(value, fileList))
                {
                    return;
                }
                fileList = value;
                OnPropertyChanged();
            }
        }

        private void LoadData(int testId)
        {
            using (var db = new UnitOfWork())
            {
                FileList = new AuxiliaryDataListViewModel(
                    db.TestRepository.Get(includeProperties: nameof(AuxilaryData))
                    .FirstOrDefault(t => t.Id == testId));
            }
            FileList.AuxiliaryData.CollectionChanged += (s, e) => UpdateDataModel();

            var pdc = new PositionDataController();

      bool retVal = pdc.LoadData(testId);
                  TotalDistance = pdc.TotalDistance;
                  Altitude = pdc.Altitude;
                  TotalTime = pdc.TotalTime;
      TotalIdleTime = pdc.TotalIdleTime;

      if (retVal)
      {
        positionDataMovement = pdc.PositionSpeedSeries;
        positionDataAccel = pdc.PositionAccelSeries;
        positionDataIdle = pdc.PositionIdleMovementSeries;
      }
                  UpdateDataModel();
        }


        // Key: Series -> value: should show by default.
        private List<KeyValuePair<DataSeries, bool>>  DataBundleFromAuxList()
        {
            var result = new List<KeyValuePair<DataSeries, bool>>();
            foreach (AuxilaryData auxData in FileList.GetSensorFiles())
            {
                DataBundle sensorData = SensorAuxDataHelper.AuxDataToSensorData(auxData);
                foreach (DataSeries dataSeries in sensorData.DataSeries)
                {
                    if (auxData.MimeType == AuxiliaryDataMimeTypes.GMobilabPlusBin
                        && dataSeries.Name == PulseChannelName)
                    {
                        IReadOnlyList<double> filtered = PulseCalculator.MakePulseFilterChain().Filter(dataSeries.Data);
                        var pulseSeries = new DataSeries(
                            filtered as double[] ?? filtered.ToArray(),
                            dataSeries.SampleRate,
                            dataSeries.SourceName,
                            "Pulsrate",
                            Unit.Bpm);
                        result.Add(new KeyValuePair<DataSeries, bool>(pulseSeries, true));
                    }
                    if (auxData.MimeType == AuxiliaryDataMimeTypes.Biofeedback2000Csv)
                    {
                        result.Add(new KeyValuePair<DataSeries, bool>(dataSeries, true));
                    }
                    else if (auxData.MimeType == AuxiliaryDataMimeTypes.GMobilabPlusBin
                             && dataSeries.Name == SkinConductanceChannelName)
                    {
                        result.Add(new KeyValuePair<DataSeries, bool>(
                            new DataSeries(
                                dataSeries.Data,
                                dataSeries.SampleRate,
                                dataSeries.SourceName,
                                "Hautleitwiderstand",
                                dataSeries.Unit),
                            true));
                    }
                }
            }
            return result;
        }

        private void UpdateDataModel()
        {
            List<KeyValuePair<DataSeries, bool>> bundle = DataBundleFromAuxList();
      bundle.Add(new KeyValuePair<DataSeries, bool>(positionDataMovement, true));
      bundle.Add(new KeyValuePair<DataSeries, bool>(positionDataAccel, true));
      bundle.Add(new KeyValuePair<DataSeries, bool>(positionDataIdle, true));
            DataModel = new DataBundleViewModel(bundle);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}