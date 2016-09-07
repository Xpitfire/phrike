// <summary>Implement StatisticUtil</summary>
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
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Phrike.Sensors
{
    /// <summary>
    /// The statistic util.
    /// </summary>
    public static class StatisticUtil
    {
        /// <summary>
        /// The difference.
        /// </summary>
        /// <param name="arr">
        /// It must be possible to run trough the array several times. 
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration", Justification = "OK. See documentation.")]
        public static double Difference(this IEnumerable<double> arr)
        {
            var min = arr.Min();
            var max = arr.Max();
            return max - min;
        }

        /// <summary>
        /// The sigma.
        /// </summary>
        /// <param name="arr">
        /// The arr.
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        public static double Sigma(this IEnumerable<double> arr)
        {
            var variance = Variance(arr);
            return Math.Sqrt(variance);
        }

        /// <summary>
        /// The variance.
        /// </summary>
        /// <param name="arr">
        /// It must be possible to run trough the array several times. 
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration", Justification = "OK. See documentation.")]
        public static double Variance(this IEnumerable<double> arr)
        {
            var avg = arr.Average();

            double sum = 0;
            int count = 0;

            foreach (double x in arr)
            {
                sum = sum + ((x - avg) * (x - avg));
                count++;
            }

            return sum / count;
        }

        /// <summary>
        /// The slope.
        /// </summary>
        /// <param name="arr">
        /// It must be possible to run trough the array several times. 
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        [SuppressMessage("ReSharper", "PossibleLossOfFraction", Justification = "OK. See documentation.")]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration", Justification = "OK. See documentation.")]
        public static double Slope(this IEnumerable<double> arr)
        {
            double numerator = 0;
            double denominator = 0;

            int idx = 0;
            double xAvg = (arr.Count() - 1) / 2.0;
            double yAvg = arr.Average();

            foreach (var y in arr)
            {
                numerator = numerator + ((idx - xAvg) * (y - yAvg));
                denominator = denominator + ((idx - xAvg) * (idx - xAvg));
                idx++;
            }

            return numerator / denominator;
        }

        /// <summary>
        /// The intercept.
        /// </summary>
        /// <param name="arr">
        /// It must be possible to run trough the array several times. 
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration", Justification = "OK. See documentation.")]
        public static double Intercept(this IEnumerable<double> arr)
        {
            return arr.Average() - (Slope(arr) * (arr.Count() - 1) / 2.0);
        }

        /// <summary>
        /// The correlation coefficient.
        /// </summary>
        /// <param name="arr">
        /// The arr.
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        [SuppressMessage("ReSharper", "PossibleLossOfFraction", Justification = "OK. See documentation.")]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration", Justification = "OK. See documentation.")]
        public static double CorrelationCoefficient(this IEnumerable<double> arr)
        {
            double numerator = 0;
            double denominatorX = 0;
            double denominatorY = 0;

            int idx = 0;
            double xAvg = (arr.Count() - 1) / 2.0;
            double yAvg = arr.Average();

            foreach (var y in arr)
            {
                numerator = numerator + ((idx - xAvg) * (y - yAvg));
                denominatorX = denominatorX + ((idx - xAvg) * (idx - xAvg));
                denominatorY = denominatorY + ((y - yAvg) * (y - yAvg));
                idx++;
            }

            return numerator / Math.Sqrt(denominatorX * denominatorY);
        }

        /// <summary>
        /// The determination coefficient.
        /// </summary>
        /// <param name="arr">
        /// It must be possible to run trough the array several times. 
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        [SuppressMessage("ReSharper", "PossibleLossOfFraction", Justification = "OK. See documentation.")]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration", Justification = "OK. See documentation.")]
        public static double DeterminationCoefficient(this IEnumerable<double> arr)
        {
            var r = CorrelationCoefficient(arr);
            return r * r;
        }
    }
}
