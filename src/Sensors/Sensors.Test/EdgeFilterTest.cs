// <summary> Unit-Test for EdgeFilterTest</summary>
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
    /// Class to test the EdgeFilter.
    /// </summary>
    [TestClass]
    public class EdgeFilterTest
    {
        /// <summary>
        /// Test for the EdgeFilter with negative values. 
        /// </summary>
        [TestMethod]
        public void EdgeFilterTest1()
        {
            EdgeDetectionFilter edf = new EdgeDetectionFilter(3);
            double[] unfilterdData = new double[] { 27, 31, 43, 34, 28, 62, 56, 37, 59, 35, 44 };
            double[] expectedResult = new double[] { -27, -8, 33, -43, -95, 115, 81, -62, 61, -56, 1 };
            double[] filterdData = edf.Filter(unfilterdData).ToArray();

            CollectionAssert.AreEqual(expectedResult, filterdData);
        }

        /// <summary>
        /// Test for the EdgeFilter with negative values. 
        /// </summary>
        [TestMethod]
        public void EdgeFilterTest2()
        {
            EdgeDetectionFilter edf = new EdgeDetectionFilter(3);
            double[] unfilterdData = new double[] { 284, 159, 584, 789, 168, 165, 489, 126, 568, 756 };
            double[] expectedResult = new double[] { -680, -1189, 1355, 2885, -1304, -1734, 362, -1516, 736, 1085 };
            double[] filterdData = edf.Filter(unfilterdData).ToArray();

            CollectionAssert.AreEqual(expectedResult, filterdData);
        }
    }
}
