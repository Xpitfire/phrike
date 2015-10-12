using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using NLog;
using OxyPlot;
using PlotController = Phrike.GroundControl.Controller.PlotController;

namespace Phrike.GroundControl.ViewModels
{
    class AnalysisViewModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly PlotController plotController;

        public PlotModel SensorPulsePlot { get; private set; }

        /// <summary>
        /// Create a new analysis viemodel instance and add the default plot template.
        /// </summary>
        public AnalysisViewModel()
        {
            plotController = new PlotController();
            SensorPulsePlot = plotController.CreatePlotModel();
        }

        /// <summary>
        /// Load pulse data from a selected file and add to the plot view.
        /// </summary>
        public async void LoadPulseData()
        {
            try
            {
                // start a file dialog and select a sensor file
                var dlg = new OpenFileDialog
                {
                    Filter = plotController.FileFilter,
                    InitialDirectory = Environment.CurrentDirectory
                };

                if (dlg.ShowDialog() == true)
                {
                    await Task.Run(() =>
                    {
                        plotController.LoadPlotData(SensorPulsePlot, dlg.FileName);
                        Logger.Info("Pluse Data successfully loaded!");
                    });
                }
            }
            catch (Exception e)
            {
                Logger.Error("Could not load pulse data!", e);
            }
        }
    }
}
