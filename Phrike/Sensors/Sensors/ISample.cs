// <summary>Specifies the ISample interface.</summary>
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

namespace OperationPhrike.Sensors
{
    /// <summary>
    /// Represents the data for each recorded sensor of a <see cref="ISensorHub"/>
    /// at a specific point in time.
    /// </summary>
    public interface ISample
    {
        /// <summary>
        /// Gets the time at which the sample values were recorded.
        /// </summary>
        DateTime Time { get; }

        /// <summary>
        /// Gets the values corresponding to each sensor.
        /// </summary>
        /// <remarks>
        /// The order of these sensor values is the same for
        /// each sample where the same set of sensors is enabled.
        /// </remarks>
        IReadOnlyList<ISampleData> Values { get; }
    }
}