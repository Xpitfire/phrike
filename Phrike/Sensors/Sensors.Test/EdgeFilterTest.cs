using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Phrike.Sensors.Filters;

namespace Sensors.Test
{
    [TestClass]
    public class EdgeFilterTest
    {


        [TestMethod]
        public void EdgeFilterTest1()
        {
            EdgeDetectionFilter edf = new EdgeDetectionFilter(3);
            double[] unfilterdData = new double[] { 27, 31, 43, 34, 28, 62, 56, 37, 59, 35, 44 };
            double[] expectedResult = new double[] { -27, -8, 33, -43, -95, 115, 81, -62, 61, -56, 1};
            double[] filterdData = edf.Filter(unfilterdData).ToArray();

            CollectionAssert.AreEqual(expectedResult, filterdData);
        }

        [TestMethod]
        public void EdgeFilterTest2()
        {
            EdgeDetectionFilter edf = new EdgeDetectionFilter(3);
            double[] unfilterdData = new double[] { 284, 159, 584, 789, 168, 165, 489, 126, 568, 756 };
            double[] expectedResult = new double[] {-680, -1189, 1355, 2885, -1304, -1734, 362, -1516, 736, 1085};
            double[] filterdData = edf.Filter(unfilterdData).ToArray();

            CollectionAssert.AreEqual(expectedResult, filterdData);
        }
    }
}
