using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using NLog;
using OxyPlot;
using Phrike.Sensors;
using Phrike.GroundControl.Controller;
using DataAccess;

namespace Phrike.GroundControl.ViewModels
{
  public class AnalysisViewModel
  {
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    public DataBundleViewModel DataModel { get; set; }

    public double TotalDistance { get; set; }

    public double Altitude { get; set; }

    public TimeSpan TotalTime { get; set; }

    public TimeSpan TotalIdleTime { get; set; }

    /// <summary>
    /// Create a new analysis viemodel instance and add the default plot template.
    /// </summary>
    public AnalysisViewModel()
    {
      DataBundle dataBundle = LoadData(1);
      DataModel = new DataBundleViewModel(dataBundle);
    }

    public DataBundle LoadData(int testId)
    {
      PositionDataController pdc = new PositionDataController();

      bool retVal = pdc.LoadData(testId);
      TotalDistance = pdc.TotalDistance;
      Altitude = pdc.Altitude;
      TotalTime = pdc.TotalTime;
      TotalIdleTime = pdc.TotalIdleTime;

      DataBundle dataBundle = new DataBundle();

      if(retVal)
      {
        dataBundle.DataSeries.Add(pdc.PositionSpeedSeries);
        dataBundle.DataSeries.Add(pdc.PositionAccelSeries);
        dataBundle.DataSeries.Add(pdc.PositionIdleMovementSeries);
      }
      dataBundle.DataSeries.Add(new DataSeries(new[] { 0.3, 0.0, 0.2, 0.4 }, 2, "src", "ser2", Unit.Unknown));

      return dataBundle;
    }
  }
}
