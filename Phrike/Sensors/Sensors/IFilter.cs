using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationPhrike.Sensors
{
    /// <summary>
    /// Interface for sensor filters.
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// Hands <paramref name="unfilteredData"/> over for filtering.
        /// </summary>
        /// <param name="unfilteredData">The data to filter.</param>
        /// <returns>
        /// Filtered data. This can contain less elements
        /// than <paramref name="unfilteredData"/>.
        /// </returns>
        IEnumerable<double> Filter(IEnumerable<double> unfilteredData);

        /// <summary>
        /// Signals the filter that the end of the is reached and filters any
        /// remaining cached data points.
        /// </summary>
        /// <returns>Any remaining cached data points, filtered.</returns>
        IEnumerable<double> Flush();

        /// <summary>
        /// Gets the radius of the filter.
        /// </summary>
        /// <remarks>
        /// The filter requires one more data point than that to start
        /// returning data.
        /// </remarks>
        int Radius { get; }
    }
}
