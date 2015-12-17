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

namespace Phrike.GroundControl.ViewModels
{
  public class AnalysisViewModel
  {
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    public DataBundleViewModel DataModel { get; set; }

    /// <summary>
    /// Create a new analysis viemodel instance and add the default plot template.
    /// </summary>
    public AnalysisViewModel()
    {
      DataBundle dataBundle = LoadData();
      DataModel = new DataBundleViewModel(dataBundle);
    }

    private DataBundle LoadData()
    {
      PositionDataController pdc = new PositionDataController();

      //pdc.LoadData(1);
      DataBundle dataBundle = new DataBundle
      {
        DataSeries =
                {
                    /*pdc.PositionSpeedSeries,
                    pdc.PositionAccelSeries,
                    pdc.PositionIdleMovementSeries*/
                    new DataSeries(new []{1.0, 2.0, 4.0, 0.0}, 2, "src", "ser", Unit.Unknown),
                    new DataSeries(new []{0.3, 0.0, 0.2, 0.4}, 2, "src", "ser2", Unit.Unknown)
                }
      };
      return dataBundle;
    }
  }
}
