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
    public class PeakFilter : RadiusFilterBase
    {
        public PeakFilter(int radius, bool detectMaxima = true) : base(radius)
        {
            this.DetectMaxima = detectMaxima;
        }

        public bool DetectMaxima { get; private set; }

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
