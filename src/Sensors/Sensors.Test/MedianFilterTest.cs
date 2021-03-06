﻿// <summary> Unit-Test for MedianFilter</summary>
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
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phrike.Sensors.Filters;

namespace Sensors.Test
{
    /// <summary>
    /// Class to test the MedianFilter.
    /// </summary>
    [TestClass]
    public class MedianFilterTest
    {
        /// <summary>
        /// Test for the MedianFilter. 
        /// </summary>
        [TestMethod]
    public void MedianFilterTest1()
    {
        MedianFilter mf = new MedianFilter(3);
        double[] unfilterdData = new double[] { 27, 31, 33, 30, 27, 26, 26, 30, 32, 35 };
        double[] expectedResult = new double[] { 31, 30, 30, 27, 30, 30, 30, 30, 30, 32 };
        double[] filterdData = mf.Filter(unfilterdData).ToArray();

        CollectionAssert.AreEqual(expectedResult, filterdData);
    }

        /// <summary>
        /// Test for the MedianFilter. 
        /// </summary>
        [TestMethod]
        public void MedianFilterTest2()
        {
            MedianFilter mf = new MedianFilter(3);
            double[] unfilterdData = new double[] { 127, 131, 313, 310, 271, 216, 126, 301, 132, 351 };
            double[] expectedResult = new double[] { 310, 271, 271, 216, 271, 271, 271, 271, 216, 301 };
            double[] filterdData = mf.Filter(unfilterdData).ToArray();

            CollectionAssert.AreEqual(expectedResult, filterdData);
        }
    }
}
