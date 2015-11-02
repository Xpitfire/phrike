using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Phrike.Sensors.Filters;

namespace Sensors.Test
{
    /// <summary>
    /// The gauss filter test.
    /// </summary>
    [TestClass]
    public class GaussFilterTest
    {
        [TestMethod]
        public void GaussFilterTest1()
        {
            GaussFilter gf = new GaussFilter(3);
            double[] unfilterdData = new double[] { 27, 31, 33, 30, 27, 26, 26, 30, 32, 35 };
            double[] expectedResult = new double[]
                                      {
                                          29.5884748176363, 30.1886940269378, 30.2227352640283, 29.409727806979,
                                          28.2463243527862, 27.5832183790343, 28.1221735847665, 29.5514602938672,
                                          31.2128306416886, 32.5818730663099
                                      };
            double[] filterdData = gf.Filter(unfilterdData).ToArray();
            for (int i = 0; i < filterdData.Length; i++)
            {
                Assert.AreEqual(expectedResult[i], filterdData[i], 0.000000001);
            }
        }

        [TestMethod]
        public void GaussFilterTest2()
        {
            GaussFilter gf = new GaussFilter(3);
            double[] unfilterdData = new double[] {4, 5, 8, -6, 8, 1, -2, 6, 5, 3, 7, 9};
            double[] expectedResult = new double[] 
                                      {
                                          4.46516731464974, 4.20113832655137, 3.4469867544489, 2.58478248709246,
                                          2.15237643888732, 2.0473810677182, 2.31260427495102, 3.26894457564313,
                                          4.22685088620187, 5.19940499081915, 6.18707186470458, 7.03616258298431
                                      };
            double[] filterdData = gf.Filter(unfilterdData).ToArray();

            for (int i = 0; i < filterdData.Length; i++)
            {
                Assert.AreEqual(expectedResult[i], filterdData[i], 0.000000001);
            }
        }
    }
}
