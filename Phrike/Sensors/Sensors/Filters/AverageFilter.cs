// <summary>Implements the AverageFilter class.</summary>
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

namespace OperationPhrike.Sensors.Filters
{
    /// <summary>
    /// Filter that returns the average of all values in the mask.
    /// </summary>
    public class AverageFilter : RadiusFilterBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AverageFilter" /> class.
        /// </summary>
        /// <param name="radius">Is passed to base constructor.</param>
        public AverageFilter(int radius)
            : base(radius)
        {
            // nothing to do
        }

        /// <inheritdoc/>
        protected override double FilterData(int start, int end, int mid, IReadOnlyList<double> unfilteredData)
        {
            double sum = 0;

            for (int i = start; i <= end; i++)
            {
                sum += unfilteredData[i];
            }

            return sum / (end - start + 1);
        }
    }
}
