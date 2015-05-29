// <summary>Implements the GaussFilter class.</summary>
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
    /// Filter that applies the bell curve over the mask.
    /// </summary>
    public class GaussFilter : RadiusFilterBase
    {
        /// <summary>
        /// Defines the width (length) of the bell curve.
        /// </summary>
        private int maskLength;

        /// <summary>
        /// Double array that contains the bell curve.
        /// </summary>
        private double[] gauss;

        /// <summary>
        /// Initializes a new instance of the <see cref="GaussFilter" /> class.
        /// </summary>
        /// <param name="radius">Is passed to base constructor.</param>
        public GaussFilter(int radius)
            : base(radius)
        {
            this.maskLength = (2 * radius) + 1;
            this.gauss = this.CalculateGaussCurve();
        }

        /// <inheritdoc/>
        protected override double FilterData(int start, int end, int mid, IReadOnlyList<double> unfilteredData)
        {
            double sum = 0;
            double gaussMaskSum = 0;

            for (int i = start; i <= end; i++)
            {
                int maskPos = i - start + (this.Radius - (mid - start));
                sum += unfilteredData[i] * this.gauss[maskPos];
                gaussMaskSum += this.gauss[maskPos];
            }

            sum /= gaussMaskSum;
            return sum;
        }

        /// <summary>
        /// Calculates the bell curve and saves it into an array.
        /// </summary>
        /// <returns>Returns the array.</returns>
        private double[] CalculateGaussCurve()
        {
            double sigma = this.Radius / 2.0;
            double sum = 0;
            this.gauss = new double[this.maskLength];
            for (int i = 0; i < this.maskLength; i++)
            {
                this.gauss[i] =
                    1 / Math.Sqrt(2 * Math.PI * sigma * sigma)
                    * Math.Exp(-(i - this.Radius)
                    * (i - this.Radius)
                    / (2 * sigma * sigma));
                sum += this.gauss[i];
            }

            for (int i = 0; i < this.maskLength; i++)
            {
                this.gauss[i] = this.gauss[i] / sum;
            }

            return this.gauss;
        }        
    }
}
