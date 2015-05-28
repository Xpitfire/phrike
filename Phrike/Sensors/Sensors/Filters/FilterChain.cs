using System.Collections.Generic;

namespace OperationPhrike.Sensors.Filters
{
    /// <summary>
    /// Maintains a chain of filters that can be applied to given (unfiltered)data.
    /// </summary>
    public class FilterChain : IFilter
    {
        /// <summary>
        /// The list of filters to apply.
        /// </summary>
        private readonly List<IFilter> filters = new List<IFilter>(); 

        /// <summary>
        /// Adds the given filter to this filter chain.
        /// </summary>
        /// <param name="filter">The filter to add.</param>
        public void Add(IFilter filter)
        {
            filters.Add(filter);
        }

        /// <summary>
        /// Applies all added filters on unfilteredData in the order they
        /// were added.
        /// </summary>
        /// <param name="unfilteredData">Data to filter.</param>
        /// <returns>The filtered data.</returns>
        public IReadOnlyList<double> Filter(IReadOnlyList<double> unfilteredData)
        {
            foreach (var filter in filters)
            {
                unfilteredData = filter.Filter(unfilteredData);
            }

            return unfilteredData;
        }
    }
}
