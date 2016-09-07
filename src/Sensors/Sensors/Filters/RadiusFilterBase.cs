// <summary>Implements the RadiusFilterBase class.</summary>
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

namespace Phrike.Sensors.Filters
{
    /// <summary>
    /// Abstract base class for filters that consider values in a given radius
    /// (the mask) from a middle value to calculate the filtered result.
    /// </summary>
    public abstract class RadiusFilterBase : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RadiusFilterBase" /> class.
        /// </summary>
        /// <param name="radius">See <see cref="Radius"/>.</param>
        protected RadiusFilterBase(int radius)
        {
            Radius = radius;
        }

        /// <summary>
        /// Gets the radius of the radiusFilter.
        /// </summary>
        /// <remarks>
        /// The radiusFilter requires one more data point than that to start
        /// returning data.
        /// </remarks>
        public int Radius { get; private set; }

        /// <summary>
        /// Hands <paramref name="unfilteredData"/> over for filtering.
        /// </summary>
        /// <param name="unfilteredData">The data to radiusFilter.</param>
        /// <returns>
        /// Filtered data. This can contain less elements
        /// than <paramref name="unfilteredData"/>.
        /// </returns>
        public virtual IReadOnlyList<double> Filter(IReadOnlyList<double> unfilteredData)
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
        /// Calculates filtered value within a range from given unfiltered data.
        /// </summary>
        /// <param name="start">Start position of mask.</param>
        /// <param name="end">End position of mask.</param>
        /// <param name="mid">Center position of mask.</param>
        /// <param name="unfilteredData">Unfiltered input data.</param>
        /// <returns>Returns filtered value for filteredData[mid].</returns>
        protected abstract double FilterData(
            int start, 
            int end, 
            int mid,
            IReadOnlyList<double> unfilteredData);
    }
}
