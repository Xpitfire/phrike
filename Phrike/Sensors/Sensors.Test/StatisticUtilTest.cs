// <summary> Unit-Test for StatisticUtil</summary>
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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sensors.Test
{
    using Phrike.Sensors;

    /// <summary>
    /// The statistic util test.
    /// </summary>
    [TestClass]
    public class StatisticUtilTest
    {
        /// <summary>
        /// The arr.
        /// </summary>
        private readonly IEnumerable<double> arr = new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        /// <summary>
        /// The difference test.
        /// </summary>
        [TestMethod]
        public void DifferenceTest()
        {
            Assert.AreEqual(9, StatisticUtil.Difference(arr));
        }

        /// <summary>
        /// The variance test.
        /// </summary>
        [TestMethod]
        public void VarianceTest()
        {
            Assert.AreEqual(8.25, StatisticUtil.Variance(arr));
        }

        /// <summary>
        /// The sigma test.
        /// </summary>
        [TestMethod]
        public void SigmaTest()
        {
            var sigma = StatisticUtil.Sigma(arr);
            var variance = StatisticUtil.Variance(arr);

            Assert.AreEqual(Math.Sqrt(variance), sigma);
        }

        /// <summary>
        /// The slope test.
        /// </summary>
        [TestMethod]
        public void SlopeTest()
        {
            Assert.AreEqual(1, StatisticUtil.Slope(arr));
        }

        /// <summary>
        /// The intercept test.
        /// </summary>
        [TestMethod]
        public void InterceptTest()
        {
            Assert.AreEqual(0, StatisticUtil.Intercept(arr));
        }

        /// <summary>
        /// The determination coefficient test.
        /// </summary>
        [TestMethod]
        public void DeterminationCoefficientTest()
        {
            Assert.AreEqual(1, StatisticUtil.DeterminationCoefficient(arr));
        }
    }
}
