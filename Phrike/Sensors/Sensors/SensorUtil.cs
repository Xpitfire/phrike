// <summary>Implements the SensorUtil class.</summary>
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

namespace OperationPhrike.Sensors
{
    /// <summary>
    /// Provides some utility functions for usage with the sensor API.
    /// </summary>
    public static class SensorUtil
    { 
        /// <summary>
        /// Get the sample values as a collection where each
        /// entry contains all values of a sensor.
        /// </summary>
        /// <param name="samples">The samples from which to retrieve the values.</param>
        /// <returns>
        /// A collection <c>v</c>, such that the items can be indexed as
        /// <c>v[sensorIndex][sampleIndex]</c>.
        /// <para>
        /// Note that the <c>sensorIndex</c> for a specific
        /// <see cref="SensorInfo"/> may be retrieved from
        /// <see cref="ISensorHub.GetSensorValueIndexInSample"/>.
        /// </para>
        /// </returns>
        public static IReadOnlyCollection<double[]> GetSampleValues(
            IEnumerable<ISample> samples)
        {
            return
                samples
                .Select(sample =>
                    sample.Values
                    .Select(sampleData => sampleData.Value)
                    .ToArray())
                .ToArray();
        }
    }
}
