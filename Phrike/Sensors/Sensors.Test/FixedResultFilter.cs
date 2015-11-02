// <summary></summary>
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
namespace Sensors.Test
{
    using System.Collections.Generic;

    using Phrike.Sensors.Filters;

    /// <summary>
    /// The fixed result filter.
    /// </summary>
    public class FixedResultFilter : IFilter
    {
        /// <summary>
        /// The return list.
        /// </summary>
        private IReadOnlyList<double> returnList;

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedResultFilter"/> class.
        /// </summary>
        /// <param name="inList">
        /// The in list.
        /// </param>
        public FixedResultFilter(IReadOnlyList<double> inList)
        {
            this.returnList = inList;
        }

        /// <summary>
        /// The filter.
        /// </summary>
        /// <param name="unfilteredData">
        /// The unfiltered data.
        /// </param>
        /// <returns>
        /// The <see cref="IReadOnlyList"/>.
        /// </returns>
        public IReadOnlyList<double> Filter(IReadOnlyList<double> unfilteredData)
        {
            return returnList;
        }
    }
}