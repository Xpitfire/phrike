using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Phrike.Sensors.Filters;

namespace Sensors.Test
{

    public class RadiusFilterBaseDerive : RadiusFilterBase
    {
        public List<int> Start { get; set; }
        public List<int> Mid { get; set; }
        public List<int> End { get; set; }


        public RadiusFilterBaseDerive(int radius) : base(radius)
        {
            Start = new List<int>();
            Mid = new List<int>();
            End = new List<int>();
        }

        protected override double FilterData(int start, int end, int mid, IReadOnlyList<double> unfilteredData)
        {

            this.Start.Add(start);
            this.Mid.Add(mid);
            this.End.Add(end);
            return unfilteredData[mid];
        }
    }

    [TestClass]
    public class RadiusFilterBaseTest
    {

        private double[] unfilteredData = 
            new double[] { 0, 1, 1, 0, 1, 0 };

        [TestMethod]
        public void RadiusFilterBaseDeriveTest1()
        {
            RadiusFilterBaseDerive rfbd = new RadiusFilterBaseDerive(3);

            var x = rfbd.Filter(unfilteredData);

            List<int> expectedStart = new List<int> {0,0,0,0,1,2};
            List<int> expectedMid = new List<int> {0,1,2,3,4,5};
            List<int> expectedEnd = new List<int> {3,4,5,5,5,5};

            CollectionAssert.AreEqual(expectedStart, rfbd.Start);
            CollectionAssert.AreEqual(expectedMid, rfbd.Mid);
            CollectionAssert.AreEqual(expectedEnd, rfbd.End);

        }


        [TestMethod]
        public void RadiusFilterBaseDeriveTest2()
        {
            RadiusFilterBaseDerive rfbd = new RadiusFilterBaseDerive(2);

            var x = rfbd.Filter(unfilteredData);

            List<int> expectedStart = new List<int> { 0, 0, 0, 1, 2, 3 };
            List<int> expectedMid = new List<int> { 0, 1, 2, 3, 4, 5 };
            List<int> expectedEnd = new List<int> { 2, 3, 4, 5, 5, 5};

            CollectionAssert.AreEqual(expectedStart, rfbd.Start);
            CollectionAssert.AreEqual(expectedMid, rfbd.Mid);
            CollectionAssert.AreEqual(expectedEnd, rfbd.End);
        }

        [TestMethod]
        public void RadiusFilterBaseDeriveTest3()
        {
            RadiusFilterBaseDerive rfbd = new RadiusFilterBaseDerive(3);

            var filteredData = rfbd.Filter(unfilteredData);
            
            CollectionAssert.AreEqual(filteredData.ToArray(), unfilteredData);
        }


        [TestMethod]
        public void RadiusFilterBaseDeriveTest4()
        {
            RadiusFilterBaseDerive rfbd = new RadiusFilterBaseDerive(0);

            var x = rfbd.Filter(unfilteredData);

            List<int> expectedStart = new List<int> { 0, 1, 2, 3, 4, 5};
            List<int> expectedMid = new List<int> { 0, 1, 2, 3, 4, 5 };
            List<int> expectedEnd = new List<int> { 0, 1, 2, 3, 4, 5 };

            CollectionAssert.AreEqual(expectedStart, rfbd.Start);
            CollectionAssert.AreEqual(expectedMid, rfbd.Mid);
            CollectionAssert.AreEqual(expectedEnd, rfbd.End);
        }

        [TestMethod]
        public void RadiusFilterBaseDeriveTest5()
        {
            RadiusFilterBaseDerive rfbd = new RadiusFilterBaseDerive(1);

            var x = rfbd.Filter(unfilteredData);

            List<int> expectedStart = new List<int> { 0, 0, 1, 2, 3, 4 };
            List<int> expectedMid = new List<int> { 0, 1, 2, 3, 4, 5 };
            List<int> expectedEnd = new List<int> { 1, 2, 3, 4, 5, 5 };

            CollectionAssert.AreEqual(expectedStart, rfbd.Start);
            CollectionAssert.AreEqual(expectedMid, rfbd.Mid);
            CollectionAssert.AreEqual(expectedEnd, rfbd.End);
        }

        [TestMethod]
        public void RadiusFilterBaseDeriveTest6()
        {
            RadiusFilterBaseDerive rfbd = new RadiusFilterBaseDerive(7);

            var x = rfbd.Filter(unfilteredData);

            List<int> expectedStart = new List<int> { 0, 0, 0, 0, 0, 0 };
            List<int> expectedMid = new List<int> { 0, 1, 2, 3, 4, 5 };
            List<int> expectedEnd = new List<int> { 5, 5, 5, 5, 5, 5 };

            CollectionAssert.AreEqual(expectedStart, rfbd.Start);
            CollectionAssert.AreEqual(expectedMid, rfbd.Mid);
            CollectionAssert.AreEqual(expectedEnd, rfbd.End);
        }

    }
}
