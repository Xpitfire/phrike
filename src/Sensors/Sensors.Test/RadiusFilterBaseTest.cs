// <summary> Unit-Test for RadiusFilterBase</summary>
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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phrike.Sensors.Filters;

namespace Sensors.Test
{
    /// <summary>
    /// Derived Class to test the RadiusFilterBase.
    /// </summary>
    public class RadiusFilterBaseDerive : RadiusFilterBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RadiusFilterBaseDerive"/> class.
        /// </summary>
        /// <param name="radius">
        /// Init with the given radius for the filters.
        /// </param>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1603:DocumentationMustContainValidXml", Justification = "Reviewed. Suppression is OK here.")]
        public RadiusFilterBaseDerive(int radius) : base(radius)
        {
            this.Start = new List<int>();
            this.Mid = new List<int>();
            this.End = new List<int>();
        }

        /// <summary> 
        /// Gets or sets the startvalues. 
        /// </summary> 
        public List<int> Start { get; set; }

        /// <summary> 
        /// Gets or sets the midvalues. 
        /// </summary> 
        /// <summary>
        /// List with the midvalues.
        /// </summary>
        public List<int> Mid { get; set; }

        /// <summary> 
        /// Gets or sets the endvalues. 
        /// </summary> 
        public List<int> End { get; set; }

        /// <summary>  Test for the BinaryTresholdFilter with negative values. 
        /// </summary>
        /// <param name="start">
        /// startvalue 
        /// </param>
        /// <param name="end">
        /// endvalue 
        /// </param>
        /// <param name="mid">
        /// midvalue 
        /// </param>
        /// <param name="unfilteredData">
        /// List with the unfilterd data 
        /// </param>
        /// <returns>
        /// value on the index mid <see cref="double"/>.
        /// </returns>
        protected override double FilterData(int start, int end, int mid, IReadOnlyList<double> unfilteredData)
        {
            this.Start.Add(start);
            this.Mid.Add(mid);
            this.End.Add(end);
            return unfilteredData[mid];
        }
    }

    /// <summary>
    /// Class for the RadiusFilterBaseDerive. 
    /// </summary>
    [TestClass]
    public class RadiusFilterBaseTest
    {
        /// <summary>
        /// Array with the unfilterd data. 
        /// </summary>
        private double[] unfilteredData = new double[] { 0, 1, 1, 0, 1, 0 };

        /// <summary>
        /// Test for the RadiusFilterBaseDerive. 
        /// </summary>
        [TestMethod]
        public void RadiusFilterBaseDeriveTest1()
        {
            RadiusFilterBaseDerive rfbd = new RadiusFilterBaseDerive(3);

            rfbd.Filter(unfilteredData);

            List<int> expectedStart = new List<int> { 0, 0, 0, 0, 1, 2 };
            List<int> expectedMid = new List<int> { 0, 1, 2, 3, 4, 5 };
            List<int> expectedEnd = new List<int> { 3, 4, 5, 5, 5, 5 };

            CollectionAssert.AreEqual(expectedStart, rfbd.Start);
            CollectionAssert.AreEqual(expectedMid, rfbd.Mid);
            CollectionAssert.AreEqual(expectedEnd, rfbd.End);
        }

        /// <summary>
        /// Test for the RadiusFilterBaseDerive. 
        /// </summary>
        [TestMethod]
        public void RadiusFilterBaseDeriveTest2()
        {
            RadiusFilterBaseDerive rfbd = new RadiusFilterBaseDerive(2);

            rfbd.Filter(unfilteredData);

            List<int> expectedStart = new List<int> { 0, 0, 0, 1, 2, 3 };
            List<int> expectedMid = new List<int> { 0, 1, 2, 3, 4, 5 };
            List<int> expectedEnd = new List<int> { 2, 3, 4, 5, 5, 5 };

            CollectionAssert.AreEqual(expectedStart, rfbd.Start);
            CollectionAssert.AreEqual(expectedMid, rfbd.Mid);
            CollectionAssert.AreEqual(expectedEnd, rfbd.End);
        }

        /// <summary>
        /// Test for the RadiusFilterBaseDerive. 
        /// </summary>
        [TestMethod]
        public void RadiusFilterBaseDeriveTest3()
        {
            RadiusFilterBaseDerive rfbd = new RadiusFilterBaseDerive(3);

            var filteredData = rfbd.Filter(unfilteredData);
            
            CollectionAssert.AreEqual(filteredData.ToArray(), unfilteredData);
        }

        /// <summary>
        /// Test for the RadiusFilterBaseDerive. 
        /// </summary>
        [TestMethod]
        public void RadiusFilterBaseDeriveTest4()
        {
            RadiusFilterBaseDerive rfbd = new RadiusFilterBaseDerive(0);

            rfbd.Filter(unfilteredData);

            List<int> expectedStart = new List<int> { 0, 1, 2, 3, 4, 5 };
            List<int> expectedMid = new List<int> { 0, 1, 2, 3, 4, 5 };
            List<int> expectedEnd = new List<int> { 0, 1, 2, 3, 4, 5 };

            CollectionAssert.AreEqual(expectedStart, rfbd.Start);
            CollectionAssert.AreEqual(expectedMid, rfbd.Mid);
            CollectionAssert.AreEqual(expectedEnd, rfbd.End);
        }

        /// <summary>
        /// Test for the RadiusFilterBaseDerive. 
        /// </summary>
        [TestMethod]
        public void RadiusFilterBaseDeriveTest5()
        {
            RadiusFilterBaseDerive rfbd = new RadiusFilterBaseDerive(1);

            rfbd.Filter(unfilteredData);

            List<int> expectedStart = new List<int> { 0, 0, 1, 2, 3, 4 };
            List<int> expectedMid = new List<int> { 0, 1, 2, 3, 4, 5 };
            List<int> expectedEnd = new List<int> { 1, 2, 3, 4, 5, 5 };

            CollectionAssert.AreEqual(expectedStart, rfbd.Start);
            CollectionAssert.AreEqual(expectedMid, rfbd.Mid);
            CollectionAssert.AreEqual(expectedEnd, rfbd.End);
        }

        /// <summary>
        /// Test for the RadiusFilterBaseDerive. 
        /// </summary>
        [TestMethod]
        public void RadiusFilterBaseDeriveTest6()
        {
            RadiusFilterBaseDerive rfbd = new RadiusFilterBaseDerive(7);

            rfbd.Filter(unfilteredData);

            List<int> expectedStart = new List<int> { 0, 0, 0, 0, 0, 0 };
            List<int> expectedMid = new List<int> { 0, 1, 2, 3, 4, 5 };
            List<int> expectedEnd = new List<int> { 5, 5, 5, 5, 5, 5 };

            CollectionAssert.AreEqual(expectedStart, rfbd.Start);
            CollectionAssert.AreEqual(expectedMid, rfbd.Mid);
            CollectionAssert.AreEqual(expectedEnd, rfbd.End);
        }
    }
}
