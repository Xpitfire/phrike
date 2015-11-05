using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Phrike.Sensors.Filters;

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
        private readonly IEnumerable<double> arr = new double[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

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
