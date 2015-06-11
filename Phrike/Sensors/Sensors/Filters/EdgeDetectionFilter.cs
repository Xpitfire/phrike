// <summary>Implements the EdgeDetectionFilter class.</summary>
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

namespace Phrike.Sensors.Filters
{
    /// <summary>
    /// Filter that intensivies peaks from the unfiltered data. 
    /// </summary>
    public class EdgeDetectionFilter : RadiusFilterBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeDetectionFilter" /> class.
        /// </summary>
        /// <param name="radius">Is passed to base constructor.</param>
        public EdgeDetectionFilter(int radius)
            : base(radius)
        {
            // nothing to do
        }

        /// <inheritdoc/>
        protected override double FilterData(int start, int end, int mid, IReadOnlyList<double> unfilteredData)
        {
            int midFactor = end - start;

            double sum = 0;
            
            for (int i = start; i <= end; i++)
            {
                if (i == mid)
                {
                    sum += unfilteredData[i] * midFactor;
                }
                else
                {
                    sum -= unfilteredData[i];
                }
            }

            return sum;
        }
    }
}
