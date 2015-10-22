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
        public static DataBundle FromHub(ISensorHub hub)
        {
            var result = new DataBundle();
            var samples = hub.ReadSamples().ToArray();
            var transposdSamples
                = new List<double>[hub.Sensors.Count(s => s.Enabled)];
            for (int i = 0; i < transposdSamples.Length; ++i)
            {
                transposdSamples[i] = new List<double>();
            }

            foreach (var sample in samples)
            {
                for (int i = 0; i < sample.SensorValues.Count; ++i)
                {
                    transposdSamples[i].Add(sample.SensorValues[i]);
                }
            }

            var sensors = new SensorInfo[transposdSamples.Length];
            for (int i = 0; i < hub.Sensors.Count; ++i)
            {
                if (hub.Sensors[i].Enabled)
                {
                    sensors[hub.GetSensorValueIndexInSample(hub.Sensors[i])]
                        = hub.Sensors[i];
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

        public IList<DataSeries> DataSeries { get; } = new List<DataSeries>();
    }
}