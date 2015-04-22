// <summary>Specifies interfaces for sensor data sources.</summary>
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
using System.Linq;

using OperationPhrike.GMobiLab;

namespace OperationPhrike.GMobiLab
{
    /// <summary>
    /// A Sensor data source (file or BT-Transfer).
    /// </summary>
    public interface ISensorDataSource : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether the data comes from a dynamic
        /// source (like the gMobiLab BT-Transfer API) or from a static one
        /// like a binary file.
        /// </summary>
        /// <remarks>
        /// If this is true, you can cast to IDynamicSensorDataSource.
        /// </remarks>
        bool IsDynamic { get; }

        /// <summary>
        /// Gets an array of 8 nullable analog channels. If null, they are not
        /// scanned.
        /// </summary>
        SensorChannel?[] AnalogChannels { get; }

        /// <summary>
        /// Gets the direction and enabledness of the 8 digital channels.
        /// </summary>
        DigitalChannelDirection[] DigitalChannels { get; }

        /// <summary>
        /// Gets the number of data points available.
        /// </summary>
        /// <remarks>
        /// If <see cref="IsDynamic"/> is true, this can increase
        /// at any time by another thread.
        /// </remarks>
        /// <returns>
        /// The number of data points readily available right now.
        /// </returns>
        int GetAvailableDataCount();

        /// <summary>
        /// Retrieve at most <paramref name="maxCount"/> data points.
        /// </summary>
        /// <param name="maxCount">
        /// Maximum number of data points to retrieve.
        /// </param>
        /// <returns>
        /// The raw sensor data as an array with at most
        /// <paramref name="maxCount"/> elements.
        /// </returns>
        short[] GetData(int maxCount);
    }
}

/// <summary>
/// Contains static helper methods for ISensorDataSource.
/// </summary>
public static class SensorDataSourceHelpers
{
    /// <summary>
    /// Counts how many shorts there are per sample.
    /// </summary>
    /// <param name="sensor">The data source to query.</param>
    /// <returns>How many shorts the result of
    /// <see cref="ISensorDataSource.GetData"/> holds per sample.
    /// </returns>
    public static int GetSampleShortCount(this ISensorDataSource sensor)
    {
        var nAnalog = sensor.AnalogChannels.Where(c => c.HasValue).Count();
        var digitalChannelEnabled = sensor.DigitalChannels.Any(
            c => c != DigitalChannelDirection.Disabled);
        return digitalChannelEnabled ? nAnalog + 1 : nAnalog;
    }
}
