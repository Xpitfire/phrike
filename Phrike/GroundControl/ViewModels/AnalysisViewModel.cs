using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using OperationPhrike.GMobiLab;
using OxyPlot;
using OxyPlot.Series;
using Phrike.GroundControl.Model;

namespace Phrike.GroundControl.ViewModels
{
    class AnalysisViewModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public PlotModel SensorPulse { get; private set; }

        public AnalysisViewModel()
        {
            this.SensorPulse = new PlotModel { Title = "Pulse Result" };
            this.SensorPulse.Series.Add(SensorsModel.GetPulseSeries());
        }

    }
}
