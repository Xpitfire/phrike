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

using System;
using System.Linq;

namespace Phrike.Sensors
{
    /// <summary>
    /// Caches statistical properties of a <see cref="DataSeries"/>.
    /// </summary>
    public class Statistics
    {
        /// <summary>
        /// The average value of the data.
        /// </summary>
        public double Average { get; set; }

        /// <summary>
        /// The minimum value of the data.
        /// </summary>
        public double Min { get; set; }

        /// <summary>
        /// The maximum value of the data.
        /// </summary>
        public double Max { get; set; }

        /// <summary>
        /// The variance (sigma^2) of the data.
        /// </summary>
        public double Variance { get; set; }

        /// <summary>
        /// The slope of the linear regression of the data.
        /// </summary>
        public double Slope { get; set; }

        /// <summary>
        /// The y offset at the beginning of the linear regression line of this data.
        /// </summary>
        public double Intercept { get; set; }

        /// <summary>
        /// The determination coefficient (r^2) of the data.
        /// </summary>
        public double DeterminationCoefficient { get; set; }

        public static Statistics FromDataSeries(DataSeries dataSeries)
        {
            return new Statistics
            {
                Min = dataSeries.Data.Min(),
                Max = dataSeries.Data.Max(),
                Average = dataSeries.Data.Average(),
                Variance = dataSeries.Data.Variance(),
                Slope = dataSeries.Data.Slope(),
                Intercept = dataSeries.Data.Intercept(),
                DeterminationCoefficient = dataSeries.Data.DeterminationCoefficient()
            };
        }
    }
}