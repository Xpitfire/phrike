using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using NLog;
using OxyPlot;
using Phrike.GroundControl.Model;

namespace Phrike.GroundControl.ViewModels
{
    class AnalysisViewModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private PlotModel sensorPulsePlotModel;

        public PlotModel SensorPulsePlotModel
        {
            get { return sensorPulsePlotModel; }
            private set { sensorPulsePlotModel = value; }
        }

        public AnalysisViewModel()
        {
            this.SensorPulsePlotModel = new PlotModel();
        }

        public void LoadPulseData()
        {
            try
            {
                var dlg = new OpenFileDialog
                {
                    Filter = "Binary files (*.bin)|*.bin",
                    InitialDirectory = Environment.CurrentDirectory
                };

                if (dlg.ShowDialog() == true)
                {
                    this.SensorPulsePlotModel.Title = "Pulse Result";
                    this.SensorPulsePlotModel.Series.Add(SensorsModel.GetPulseSeries(dlg.FileName, false));
                    this.SensorPulsePlotModel.InvalidatePlot(true);
                    Logger.Info("Pluse Data successfully loaded!");
                }
            }
            catch (Exception e)
            {
                Logger.Error("Could not load pulse data!", e);
            }
        }
    }
}
