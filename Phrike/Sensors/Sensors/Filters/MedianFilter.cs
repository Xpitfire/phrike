using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phrike.Sensors.Filters
{
    /// <summary>
    /// Filter that returns the median of all values in the mask.
    /// </summary>
    public class MedianFilter : RadiusFilterBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MedianFilter" /> class.
        /// </summary>
        /// <param name="radius">Is passed to base constructor.</param>
        public MedianFilter(int radius)
            : base(radius)
        {
            //nothing to do
        }

        /// <inheritdoc/>
        protected override double FilterData(int start, int end, int mid, IReadOnlyList<double> unfilteredData)
        {
            double[] sort = unfilteredData
                .Skip(start)
                .Take(end - start + 1)
                .ToArray();
            Array.Sort(sort);

            return sort[(end - start + 1) / 2];
        }
    }
}
