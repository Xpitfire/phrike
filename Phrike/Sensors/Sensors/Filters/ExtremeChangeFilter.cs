// <summary>Implements the ExtremeChangeFilter class.</summary>
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

namespace OperationPhrike.Sensors.Filters
{
    /// <summary>
    /// Filter that locks the y-Axis at the last value
    /// if the difference of two consecutive values
    /// exceeds MaxYDistance.
    /// </summary>
    public class ExtremeChangeFilter : RadiusFilterBase
    {
        /// <summary>
        /// The last valid value (before the last too extreme change if any).
        /// </summary>
        private double lastValidVal;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtremeChangeFilter" /> class.
        /// The radius "1"  is passed to base constructor.
        /// </summary>
        /// <param name="lastValidVal"></param>
        /// <param name="maxYDistance">See <see cref="MaxYDistance"/>.</param>
        public ExtremeChangeFilter(int lastValidVal, double maxYDistance)
            : base(1)
        {
            this.lastValidVal = lastValidVal;
            this.MaxYDistance = maxYDistance;
        }

        /// <summary>
        /// Gets the maximum distance of two consecutive datapoints.
        /// </summary>
        public double MaxYDistance { get; private set; }

        /// <inheritdoc/>
        protected override double FilterData(int start, int end, int mid, IReadOnlyList<double> unfilteredData)
        {
            if (Math.Abs(unfilteredData[mid] - this.lastValidVal) > this.MaxYDistance)
            {
                return this.lastValidVal;
            }

            this.lastValidVal = unfilteredData[mid];
            return unfilteredData[mid];
        }
    }
}
