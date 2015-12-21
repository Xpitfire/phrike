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

using NLog;

using Phrike.GroundControl.Controller;
using Phrike.Sensors;

namespace Phrike.GroundControl.ViewModels
{
    public class AnalysisViewModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///     Create a new analysis viemodel instance and add the default plot template.
        /// </summary>
        public AnalysisViewModel()
        {
            DataBundle dataBundle = LoadData(1);
            DataModel = new DataBundleViewModel(dataBundle);
        }

        public DataBundleViewModel DataModel { get; set; }

        public double TotalDistance { get; set; }

        public double Altitude { get; set; }

        public TimeSpan TotalTime { get; set; }

        public TimeSpan TotalIdleTime { get; set; }

        private DataBundle LoadData(int testId)
        {
            var pdc = new PositionDataController();

            /*pdc.LoadData(1);
      TotalDistance = pdc.TotalDistance;
      Altitude = pdc.Altitude;
      TotalTime = pdc.TotalTime;
      TotalIdleTime = pdc.TotalIdleTime;*/

            var dataBundle = new DataBundle
            {
                DataSeries =
                {
                    /*pdc.PositionSpeedSeries,
                    pdc.PositionAccelSeries,
                    pdc.PositionIdleMovementSeries*/
                    new DataSeries(
                        new[] { 1.0, 2.0, 4.0, 0.0 },
                        2,
                        "src",
                        "ser",
                        Unit.Unknown),
                    new DataSeries(
                        new[] { 0.3, 0.0, 0.2, 0.4 },
                        2,
                        "src",
                        "ser2",
                        Unit.Unknown)
                }
            };
            return dataBundle;
        }
    }
}