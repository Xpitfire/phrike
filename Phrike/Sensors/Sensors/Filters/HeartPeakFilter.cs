// <summary>Implementation of HeartPeakFilter.</summary>
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
using System.Diagnostics.CodeAnalysis;

namespace Phrike.Sensors.Filters
{
    /// <summary>
    /// Filters heart peaks by only retaining maximum peaks that are followed
    /// by minimum peaks.
    /// </summary>
    public class HeartPeakFilter : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="HeartPeakFilter"/> class.
        /// </summary>
        /// <param name="maxPeakFilter">
        /// See <see cref="MaxPeakFilter"/>.
        /// </param>
        /// <param name="minPeaksFilter">
        /// See <see cref="MinPeakFilter"/>.
        /// </param>
        /// <param name="maxPeakDistance">
        /// See <see cref="MaxPeakDistance"/>.
        /// </param>
        public HeartPeakFilter(
            IFilter maxPeakFilter, IFilter minPeaksFilter, int maxPeakDistance)
        {
            MaxPeakFilter = maxPeakFilter;
            MinPeakFilter = minPeaksFilter;
            MaxPeakDistance = maxPeakDistance;
        }

        /// <summary>
        /// Gets the filter used to obtain the maximum peaks.
        /// </summary>
        public IFilter MaxPeakFilter { get; private set; }

        /// <summary>
        /// Gets the filter used to obtain the minimum peaks.
        /// </summary>
        public IFilter MinPeakFilter { get; private set; }

        /// <summary>
        /// Gets the maximum distance between a maximum and a minimum peak.
        /// Maximum peaks that are not followed by a minimum peak within
        /// this distance are discarded.
        /// </summary>
        public int MaxPeakDistance { get; private set; }

        /// <summary>
        /// Merge minimum and maximum peaks, like <see cref="Filter"/>.
        /// This is an alternative interface you can use if you have already
        /// computed minimum and maximum peaks.
        /// </summary>
        /// <param name="maxPeaks">
        /// Output of a <see cref="PeakFilter"/> with maximum peaks.
        /// </param>
        /// <param name="minPeaks">
        /// Output of a <see cref="PeakFilter"/> with minimum peaks.
        /// </param>
        /// <param name="maxPeakDistance">
        /// See <see cref="MaxPeakDistance"/>.
        /// </param>
        /// <returns>
        /// The maximum peaks followed within
        /// <paramref name="maxPeakDistance"/> with a minimum peaks amplified,
        /// the others discarded.
        /// </returns>
        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator",
            Justification =
                "PeakFilter sets the values in question to exactly"
                + "zero, so comparison should be fine.")]
        public static IReadOnlyList<double> MergePeaks(
            IReadOnlyList<double> maxPeaks,
            IReadOnlyList<double> minPeaks,
            int maxPeakDistance)
        {
            double[] result = new double[maxPeaks.Count];
            for (int i = 0; i < maxPeaks.Count; ++i)
            {
                if (maxPeaks[i] != 0)
                {
                    bool found = false;
                    for (int j = i; j < i + maxPeakDistance && j < maxPeaks.Count && !found; ++j)
                    {
                        if (minPeaks[j] != 0)
                        {
                            result[i] = maxPeaks[i] - minPeaks[j];
                            found = true;
                        }
                    }
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public IReadOnlyList<double> Filter(IReadOnlyList<double> unfilteredData)
        {
            ////const int MaxSearchDistance = 11;
            IReadOnlyList<double> maxPeaks = MaxPeakFilter.Filter(unfilteredData);
            IReadOnlyList<double> minPeaks = MinPeakFilter.Filter(unfilteredData);
            return MergePeaks(maxPeaks, minPeaks, MaxPeakDistance);
        }
    }
}
