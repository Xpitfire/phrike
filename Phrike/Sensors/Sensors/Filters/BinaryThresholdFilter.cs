// <summary>Implementation of the BinaryThresholdFilter.</summary>
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
using System.Linq;

namespace OperationPhrike.Sensors.Filters
{
    /// <summary>
    /// Filter that sets all values that fall below (DetectMaxima == true)
    /// or exceed (!DetectMaxima) the value m * ThresholdRatio to zero,
    /// where m is the median of the the top (DetectMaxima == true) or
    /// bottom (!DetectMaxima) unfilteredData.Count * TopShare values.
    /// </summary>
    public class BinaryThresholdFilter : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="BinaryThresholdFilter"/> class.
        /// </summary>
        /// <param name="thresholdRatio">See <see cref="ThresholdRatio"/>.</param>
        /// <param name="detectMaxima">See <see cref="DetectMaxima"/>.</param>
        /// <param name="topShare">See <see cref="TopShare"/>.</param>
        public BinaryThresholdFilter(
            double thresholdRatio,
            bool detectMaxima = true,
            double topShare = 1 / 500.0)
        {
            ThresholdRatio = thresholdRatio;
            DetectMaxima = detectMaxima;
            TopShare = topShare;
        }

        /// <summary>
        /// Gets a value indicating whether to retain values that exceed
        /// the median of the selected top (true) or fall below the median of
        /// the selected bottom (false).
        /// </summary>
        public bool DetectMaxima { get; private set; }


        /// <summary>
        /// Gets the factor with which the median is multiplied to obtain the
        /// cutoff value.
        /// </summary>
        public double ThresholdRatio { get; private set; }

        /// <summary>
        /// Gets the share of the sorted input values that will be used to
        /// calculate the median.
        /// </summary>
        public double TopShare { get; private set; }

        /// <inheritdoc/>
        public IReadOnlyList<double> Filter(IReadOnlyList<double> unfilteredData)
        {
            var targetNumber = (int)(unfilteredData.Count * TopShare);
            double referenceValue = unfilteredData
                .OrderByDescending(v => DetectMaxima ? v : -v)
                .Skip(targetNumber / 2)
                .First();
            double cutoffMin = referenceValue * ThresholdRatio;
            var result = new double[unfilteredData.Count];

            for (int i = 0; i < unfilteredData.Count; ++i)
            {
                if ((unfilteredData[i] > cutoffMin && DetectMaxima)
                    || (unfilteredData[i] < cutoffMin && !DetectMaxima))
                {
                    result[i] = unfilteredData[i];
                }
            }

            return result;
        }
    }
}
