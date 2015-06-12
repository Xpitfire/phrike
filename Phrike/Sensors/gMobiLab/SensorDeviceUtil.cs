// <summary>Implementation of the SensorDeviceUtil class.</summary>
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

using System.Linq;
using Phrike.GMobiLab;
using Phrike.Sensors;
using Phrike.Sensors.Filters;

namespace Phrike.GMobiLab
{
    /// <summary>
    /// This class is used to get and convert data samples from sensor generated binary files.
    /// </summary>
    public static class SensorDeviceUtil
    {
        /// <summary>
        /// Returns the entire data-stream from a file.
        /// </summary>
        /// <param name="fileName">Name of the binary data sample file.</param>
        /// <returns>ISample[] with all sensor channels.</returns>
        public static ISample[] GetSamples(string fileName)
        {
            ISensorHub dataSource = new SensorDataFileStreamer(fileName);
            return dataSource.ReadSamples().ToArray();
        }

        /// <summary>
        /// Extracts the puls channel data  information from the entire sample collection.
        /// </summary>
        /// <param name="dataSamples">Samples with the entire channel data information.</param>
        /// <returns>An array of raw / unfiltered pulse data values.</returns>
        public static double[] GetPulseRawData(ISample[] dataSamples)
        {
            return SensorUtil.GetSampleValues(dataSamples, 4).ToArray();
        }

        /// <summary>
        /// Filters the raw pulse data samples.
        /// </summary>
        /// <param name="rawData">Raw / unfiltered data values.</param>
        /// <returns>An array of filtered pulse data values.</returns>
        public static double[] GetPulseFilteredData(double[] rawData)
        {
            return PulseCalculator.MakePulseFilterChain().Filter(rawData).ToArray();
        }
    }
}
