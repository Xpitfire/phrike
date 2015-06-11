// <summary>Implements the BasicSampleData class.</summary>
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

namespace Phrike.Sensors
{
    /// <summary>
    /// A simple <see cref="ISampleData"/> implementation that just
    /// saves the property values in the constructor.
    /// </summary>
    public class BasicSampleData : ISampleData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicSampleData"/> class.
        /// </summary>
        /// <param name="source">
        /// Value for <see cref="Source"/>.
        /// </param>
        /// <param name="value">
        /// Value for <see cref="Value"/>.
        /// </param>
        public BasicSampleData(SensorInfo source, double value)
        {
            Source = source;
            Value = value;
        }

        /// <inheritdoc/>
        public SensorInfo Source { get; private set; }

        /// <inheritdoc/>
        public double Value { get; private set; }
    }
}
