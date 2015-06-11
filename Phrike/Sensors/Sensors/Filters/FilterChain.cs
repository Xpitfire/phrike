using System.Collections.Generic;

namespace Phrike.Sensors.Filters
{
    /// <summary>
    /// Maintains a chain of filters that can be applied to given (unfiltered)data.
    /// </summary>
    public class FilterChain : IFilter
    {
        /// <summary>
        /// The list of filters to apply.
        /// </summary>
        private readonly List<IFilter> filters;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterChain"/> class.
        /// </summary>
        /// <param name="filters">
        /// The filters to add initially.
        /// </param>
        public FilterChain(params IFilter[] filters)
        {
            this.filters = new List<IFilter>();
            this.filters.AddRange(filters);
        }

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
