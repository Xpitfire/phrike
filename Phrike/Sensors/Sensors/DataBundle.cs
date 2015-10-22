// <summary>Implementation file for DataBundle.</summary>
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

using System.Collections.Generic;
using System.Linq;

namespace Phrike.Sensors
{
    /// <summary>
    /// Bundles multiple <see cref="DataSeries"/>.
    /// </summary>
    public class DataBundle
    {
        /// <summary>
        /// Creates a data bundle of all sensors with all currently
        /// available samples from <paramref name="hub" />.
        /// </summary>
        /// <param name="hub">The hub from which to retrieve the data.</param>
        /// <returns>A new instance of <see cref="DataBundle"/>.</returns>
        public static DataBundle FromHub(ISensorHub hub)
        {
            var result = new DataBundle();
            Sample[] samples = hub.ReadSamples().ToArray();
            var transposdSamples
                = new List<double>[hub.Sensors.Count(s => s.Enabled)];

            for (var i = 0; i < transposdSamples.Length; ++i)
            {
                transposdSamples[i] = new List<double>();
            }

            foreach (Sample sample in samples)
            {
                for (var i = 0; i < sample.SensorValues.Count; ++i)
                {
                    transposdSamples[i].Add(sample.SensorValues[i]);
                }
            }

            var sensors = new SensorInfo[transposdSamples.Length];
            foreach (SensorInfo sensorInfo in hub.Sensors)
            {
                if (sensorInfo.Enabled)
                {
                    sensors[hub.GetSensorValueIndexInSample(sensorInfo)] = sensorInfo;
                }
            }

            for (int i = 0; i < transposdSamples.Length; ++i)
            {
                result.DataSeries.Add(
                    new DataSeries(
                        transposdSamples[i].ToArray(),
                        hub.SampleRate,
                        hub.Name,
                        sensors[i].Name,
                        sensors[i].Unit));
            }

            return result;
        }

        /// <summary>
        /// The list of all bundled data series.
        /// </summary>
        public IList<DataSeries> DataSeries { get; } = new List<DataSeries>();
    }
}