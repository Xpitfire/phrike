// <summary>Implementation file for Statistics.</summary>
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

using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Phrike.Sensors
{
    /// <summary>
    /// Caches statistical properties of a <see cref="DataSeries"/>.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")] // TODO: Use!
    public class Statistics
    {
        /// <summary>
        /// Gets or sets the average value of the data.
        /// </summary>
        public double Average { get; set; }

        /// <summary>
        /// Gets or sets the minimum value of the data.
        /// </summary>
        public double Min { get; set; }

        /// <summary>
        /// Gets or sets the maximum value of the data.
        /// </summary>
        public double Max { get; set; }

        /// <summary>
        /// Gets or sets the variance (sigma^2) of the data.
        /// </summary>
        public double Variance { get; set; }

        /// <summary>
        /// Gets or sets the slope of the linear regression of the data.
        /// </summary>
        public double Slope { get; set; }

        /// <summary>
        /// Gets or sets the y offset at the beginning of the linear regression line of this data.
        /// </summary>
        public double Intercept { get; set; }

        /// <summary>
        /// Gets or sets the determination coefficient (r^2) of the data.
        /// </summary>
        public double DeterminationCoefficient { get; set; }

        /// <summary>
        /// Calculates <see cref="Statistics"/> from <paramref name="dataSeries"/>.
        /// </summary>
        /// <param name="dataSeries">The data series to calcualate the statistics from.</param>
        /// <returns>
        ///     A new instance of <see cref="Statistics"/> containing the
        ///     results from <paramref name="dataSeries"/>
        /// </returns>
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