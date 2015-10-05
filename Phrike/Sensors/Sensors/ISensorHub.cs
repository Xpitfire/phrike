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

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Phrike.Sensors
{
    /// <summary>
    ///     Represents a collection of physically related sensor data (e.g.
    ///     data from the same file or from sensor controller hardware
    ///     with multiple channels) the data of which is bundled together in
    ///     the same samples.
    /// </summary>
    public interface ISensorHub : IDisposable
    {
        /// <summary>
        /// Gets information about the available Sensors.
        /// </summary>
        IReadOnlyList<SensorInfo> Sensors { get; }

        /// <summary>
        /// Gets a value indicating whether new samples may become available.
        /// </summary>
        bool IsUpdating { get; }

        /// <summary>
        /// Gets the samplerate of this <see cref="ISensorHub"/> in Hz = 1/s.
        /// </summary>
        int SampleRate { get; }

        /// <summary>
        /// Sets which sensors should be recorded/retrieved.
        /// </summary>
        /// <param name="sensor">
        /// Which sensor to enable/disable.
        /// </param>
        /// <param name="enabled">
        /// Whether to enable or disable the sensor.
        /// </param>
        void SetSensorEnabled(SensorInfo sensor, bool enabled = true);

        /// <summary>
        ///     Gets the number of data points available.
        /// </summary>
        /// <remarks>
        ///     If <see cref="IsUpdating" /> is true, this can be increased
        ///     at any time by another thread.
        /// </remarks>
        /// <returns>
        ///     The number of data points readily available right now.
        /// </returns>
        int GetAvailableSampleCount();

        /// <summary>
        /// Returns where in the samples' values the given
        /// sensor's value is saved.
        /// </summary>
        /// <remarks>
        /// The value returned by this message for the same
        /// <paramref name="sensor"/> will change if you enable/disable
        /// sensors.
        /// </remarks>
        /// <param name="sensor">The sensor for which to retrieve the index.</param>
        /// <returns>
        /// An index <c>i</c> such that for each sample <c>s</c> returned by
        /// <see cref="ReadSamples"/>, <c>s.Values[i]</c> is the value
        /// corresponding to the <paramref name="sensor"/>.
        /// </returns>
        int GetSensorValueIndexInSample(SensorInfo sensor);

        /// <summary>
        /// Retrieve at most <paramref name="maxCount"/> data points (oldest first).
        /// </summary>
        /// <param name="maxCount">
        /// Maximum number of samples to retrieve.
        /// </param>
        /// <returns>
        /// A readonly collection of at most <paramref name="maxCount"/> samples.
        /// </returns>
        IEnumerable<Sample> ReadSamples(int maxCount = int.MaxValue);
    }
}