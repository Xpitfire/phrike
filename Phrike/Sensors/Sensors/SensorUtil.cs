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

namespace Phrike.Sensors
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
        /// <param name="sensorIdx">The index of the sensor to retrieve. See
        /// <see cref="ISensorHub.GetSensorValueIndexInSample"/>.</param>
        /// <returns>
        /// The sensors' values.
        /// </returns>
        public static double[] GetSampleValues(IEnumerable<ISample> samples, int sensorIdx)
        {
            return samples.Select(sample => sample.Values[sensorIdx].Value).ToArray();
        }
    }
}
