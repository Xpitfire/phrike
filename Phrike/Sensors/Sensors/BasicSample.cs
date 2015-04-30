// <summary>Implements the BasicSample class.</summary>
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
    /// A simple <see cref="ISample"/> implementation that just saves the
    /// property values in the constructor.
    /// </summary>
    public class BasicSample : ISample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicSample"/> class. 
        /// </summary>
        /// <param name="time">
        /// Value for <see cref="Time"/>.
        /// </param>
        /// <param name="values">
        /// Value for <see cref="Values"/>.
        /// </param>
        public BasicSample(DateTime time, IReadOnlyList<ISampleData> values)
        {
            Time = time;
            Values = values;
        }

        /// <inheritdoc/>
        public DateTime Time { get; private set; }

        /// <inheritdoc/>
        public IReadOnlyList<ISampleData> Values { get; private set; }
    }
}
