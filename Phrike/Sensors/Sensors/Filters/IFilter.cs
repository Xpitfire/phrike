// <summary>Specifies interface for a filter.</summary>
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
    /// Represents a filter that can be applied to unfiltered data.
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// Filters the given unfiltered data.
        /// </summary>
        /// <param name="unfilteredData">The unfiltered data.</param>
        /// <returns>New collection of filtered data.</returns>
        IReadOnlyList<double> Filter(IReadOnlyList<double> unfilteredData);
    }
}
