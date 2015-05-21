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
    public abstract class FilterBase
    {

        public FilterBase(int radius)
        {
            Radius = radius;
        }

        /// <summary>
        /// Hands <paramref name="unfilteredData"/> over for filtering.
        /// </summary>
        /// <param name="unfilteredData">The data to filter.</param>
        /// <returns>
        /// Filtered data. This can contain less elements
        /// than <paramref name="unfilteredData"/>.
        /// </returns>
        virtual public double[] Filter(IReadOnlyList<double> unfilteredData)
        {
            double[] filteredData = new double[unfilteredData.Count];

            for (int i = 0; i < unfilteredData.Count; i++)
            {
                filteredData[i] = FilterData(
                    Math.Max(0, i - Radius),
                    Math.Min(i + Radius, unfilteredData.Count - 1),
                    i,
                    unfilteredData);
            }

            return filteredData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="max"></param>
        /// <param name="end"></param>
        /// <param name="unfilteredData"></param>
        /// <returns></returns>
        protected abstract double FilterData(
            int start, int end, int mid,
            IReadOnlyList<double> unfilteredData);

        /// <summary>
        /// Gets the radius of the filter.
        /// </summary>
        /// <remarks>
        /// The filter requires one more data point than that to start
        /// returning data.
        /// </remarks>
        public int Radius { get; private set; }
    }
}
