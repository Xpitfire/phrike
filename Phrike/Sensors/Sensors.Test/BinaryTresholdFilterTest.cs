using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Phrike.Sensors.Filters;

namespace Sensors.Test
{
    [TestClass]
    public class BinaryTresholdFilterTest
    {
        [TestMethod]
        public void BinaryTresholdFilterTest1()
        {
            BinaryThresholdFilter btf = new BinaryThresholdFilter(3);
            double[] unfilterdData = new double[] {84, 958, 27, 310, 31, 33, 77, 30, 27, 246, 263, 300, 432, 35, 777, 37, 32};
            double[] expectedResult = new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            double[] filterdData = btf.Filter(unfilterdData).ToArray();

            for (int i = 0; i < filterdData.Length; i++)
            { 
                Assert.AreEqual(expectedResult[i], filterdData[i]);
            }
            CollectionAssert.AreEqual(expectedResult, filterdData);
        }

        [TestMethod]
        public void BinaryTresholdFilterTest2()
        {
            BinaryThresholdFilter btf = new BinaryThresholdFilter(3);
            double[] unfilterdData = new double[] {0, -1, 2, -3, 4, 5, 6, -7 };
            double[] expectedResult = new double[] { 0, 0, 0, 0, 0, 0, 0, 0};
            double[] filterdData = btf.Filter(unfilterdData).ToArray();

            for (int i = 0; i < filterdData.Length; i++)
            {
                Assert.AreEqual(expectedResult[i], filterdData[i]);
            }
            CollectionAssert.AreEqual(expectedResult, filterdData);
        }

    }
}
