// <summary>Unit-Test for HeartPeakFilter</summary>
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

namespace Sensors.Test
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Phrike.Sensors.Filters;

    /// <summary>
    /// Class to test the HeartPeakFilter.
    /// </summary>
    [TestClass]
    public class HeartPeakFilterTest
    {
        /// <summary>
        /// The max peaks.
        /// </summary>
        private readonly double[] maxPeaks = 
            new double[] { 4, 0, 0, 5, 0, 0, 0, 3, 0, 0, 0, 0, 0, 3, 0, 0, 0, 2, 3, 2, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0 };

        /// <summary>
        /// The min peaks.
        /// </summary>
        private readonly double[] minPeaks = 
            new double[] { -2, 0, 0, -4, 0, 0, 0, 0, -2, 0, 0, 0, -4, 0, 0, 0, 0, 0, -3, 0, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0 };

        /// <summary>
        /// The result.
        /// </summary>
        private readonly double[] expectedResult = 
            new double[] { 6, 0, 0, 9, 0, 0, 0, 5, 0, 0, 0, 0, 0, 7, 0, 0, 0, 5, 6, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        /// <summary>
        /// The max peak distance.
        /// </summary>
        private int maxPeakDistance = 2;

        /// <summary>
        /// The merge peaks test.
        /// </summary>
        [TestMethod]
        public void MergePeaksTest()
        {
            IReadOnlyList<double> result = HeartPeakFilter.MergePeaks(maxPeaks, minPeaks, maxPeakDistance);
            CollectionAssert.AreEqual(result.ToArray(), expectedResult);
        }

        /// <summary>
        /// The hp filter test.
        /// </summary>
        [TestMethod]
        public void HpFilterTest()
        {
            IFilter maxPeakFilter = new FixedResultFilter(maxPeaks);
            IFilter minPeakFilter = new FixedResultFilter(minPeaks);
            IFilter filter = new HeartPeakFilter(maxPeakFilter, minPeakFilter, maxPeakDistance);

            CollectionAssert.AreEqual(filter.Filter(null).ToArray(), expectedResult);
        }
    }
}