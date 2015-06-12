// <summary>Implements the PeakFilter class.</summary>
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
    /// Filter that detects peaks by finding the greatest/smallest value in the
    /// given radius.
    /// </summary>
    public class PeakFilter : RadiusFilterBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PeakFilter"/> class.
        /// </summary>
        /// <param name="radius">
        /// See <see cref="RadiusFilterBase.Radius"/>.
        /// </param>
        /// <param name="detectMaxima">
        /// See <see cref="DetectMaxima"/>.
        /// </param>
        public PeakFilter(int radius, bool detectMaxima = true) : base(radius)
        {
            this.DetectMaxima = detectMaxima;
        }

        /// <summary>
        /// Gets a value indicating whether to detect the greatest (true) or
        /// smallest (false) value as peak.
        /// </summary>
        public bool DetectMaxima { get; private set; }

        /// <inheritdoc/>
        protected override double FilterData(
            int start, int end, int mid, IReadOnlyList<double> unfilteredData)
        {
            double midVal = unfilteredData[mid];
            bool isExtreme = true;
            for (int i = start; i <= end && isExtreme; i++)
            {
               double actVal = unfilteredData[i];
                if ((actVal > midVal && this.DetectMaxima)
                    || (actVal < midVal && !this.DetectMaxima))
                {
                    isExtreme = false;
                }
            }

            return isExtreme ? midVal : 0;
        }
    }
}
