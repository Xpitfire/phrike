﻿// <summary></summary>
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

namespace Phrike.GroundControl.ViewModels
{
    public class AnalysisViewModel : INotifyPropertyChanged
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private AuxiliaryDataListViewModel fileList;

        private DataBundleViewModel dataModel;

        /// <summary>
        ///     Create a new analysis viemodel instance and add the default plot template.
        /// </summary>
        public AnalysisViewModel()
        {
            if (!DataLoadHelper.IsLoadDataActive())
                return;
            LoadData(1); // TODO get real id
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

            // TODO Store pdc data only in extra list to survive file list changes.
                /*pdc.LoadData(1);
                  TotalDistance = pdc.TotalDistance;
                  Altitude = pdc.Altitude;
                  TotalTime = pdc.TotalTime;
                  TotalIdleTime = pdc.TotalIdleTime;*/
                  UpdateDataModel();
        }

        private DataBundle DataBundleFromAuxList()
        {
            var dataBundle = new DataBundle();
            foreach (AuxilaryData auxData in FileList.GetSensorFiles())
            {
                DataBundle sensorData = SensorAuxDataHelper.AuxDataToSensorData(auxData);
                foreach (DataSeries dataSeries in sensorData.DataSeries)
                {
                    dataBundle.DataSeries.Add(dataSeries);
                }
            }
            return dataBundle;
        }

        private void UpdateDataModel()
        {
            DataBundle bundle = DataBundleFromAuxList();
            // TODO Add pdc data to bundle.
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