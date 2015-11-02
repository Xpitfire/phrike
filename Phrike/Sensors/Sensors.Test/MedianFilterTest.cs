using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Phrike.Sensors.Filters;

namespace Sensors.Test
{
    [TestClass]
    public class MedianFilterTest
    {

        [TestMethod]
    public void MedianFilterTest1()
    {
        MedianFilter mf = new MedianFilter(3);
        double[] unfilterdData = new double[] { 27, 31, 33, 30, 27, 26, 26, 30, 32, 35 };
        double[] expectedResult = new double[] {31, 30, 30, 27, 30, 30, 30, 30, 30, 32 };
        double[] filterdData = mf.Filter(unfilterdData).ToArray();

        CollectionAssert.AreEqual(expectedResult, filterdData);
    }

        [TestMethod]
        public void MedianFilterTest2()
        {
            MedianFilter mf = new MedianFilter(3);
            double[] unfilterdData = new double[] { 127, 131, 313, 310, 271, 216, 126, 301, 132, 351 };
            double[] expectedResult = new double[] {310, 271, 271, 216, 271, 271, 271, 271, 216, 301 };
            double[] filterdData = mf.Filter(unfilterdData).ToArray();

            CollectionAssert.AreEqual(expectedResult, filterdData);
        }

    }
}
