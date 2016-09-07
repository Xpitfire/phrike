// <summary>Implements the Sample class.</summary>
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

namespace Phrike.Sensors
{
    /// <summary>
    /// All sensor data available at a point in time from a particular source
    /// (usually a <see cref="ISensorHub"/>).
    /// </summary>
    public class Sample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sample"/> class.
        /// </summary>
        /// <param name="sensorValues">
        /// Value for <see cref="SensorValues"/>.
        /// </param>
        public Sample(IReadOnlyList<double> sensorValues)
        {
            SensorValues = sensorValues;
        }

        /// <summary>
        ///     Gets the values corresponding to each sensor.
        /// </summary>
        /// <remarks>
        ///     The order of these sensor values is the same for
        ///     each sample where the same set of sensors is enabled.
        /// </remarks>
        public IReadOnlyList<double> SensorValues { get; }
    }
}