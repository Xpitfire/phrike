// <summary>Specifies interfaces for a sensor hub.</summary>
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

namespace OperationPhrike.Sensors
{
    /// <summary>
    /// The unit of a Sensor's values.
    /// </summary>
    public enum Unit
    {
        /// <summary>
        /// A 1000th of a Volt (usually raw sensor data).
        /// </summary>
        MilliVolt
    }

    /// <summary>
    /// Provides immutable information about a sensor of a <see cref="ISensorHub"/>.
    /// </summary>
    public struct SensorInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SensorInfo"/> struct.
        /// </summary>
        /// <param name="name">
        /// The name of the sensor.
        /// </param>
        /// <param name="unit">
        /// The unit of the sensor's values.
        /// </param>
        /// <param name="enabled">
        /// Whehter the sensor is enabled.
        /// </param>
        /// <param name="id">
        /// The id inside the <see cref="ISensorHub"/>.
        /// </param>
        internal SensorInfo(string name, Unit unit, bool enabled, int id)
            : this()
        {
            Name = name;
            Unit = unit;
            Enabled = enabled;
            Id = id;
        }

        /// <summary>
        /// Gets a descriptive name of the sensor.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the unit of the sensor's values.
        /// </summary>
        public Unit Unit { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the sensor is enabled.
        /// </summary>
        public bool Enabled { get; private set; }

        /// <summary>
        /// Gets the ID of the sensor inside the containing <see cref="ISensorHub"/>.
        /// </summary>
        public int Id { get; private set; }
    }
}