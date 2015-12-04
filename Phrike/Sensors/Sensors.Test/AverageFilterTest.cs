// <summary> Unit-Test for AverageFilter</summary>
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
    /// Class to test the AverageFilter.
    /// </summary>
    [TestClass]
    public class AverageFilterTest
    {
        /// <summary>
        /// Test for the AverageFilter. 
        /// </summary>
        [TestMethod]
        public void AverageFilterTest1()
        {
            AverageFilter af = new AverageFilter(3);
            double[] unfilterdData = new double[] { 1, 4, 7, 4, 5, 6, 4, 3, 6 };
            double[] expectedResult = new double[] { 4, 4.2, 4.5, 4.42857142857143, 4.71428571428571, 5, 4.66666666666667, 4.8, 4.75 };
            double[] filterdData = af.Filter(unfilterdData).ToArray();

            for (int i = 0; i < filterdData.Length; i++)
            {
                Assert.AreEqual(expectedResult[i], filterdData[i], 0.0001);
            }
        }

        /// <summary>
        /// Test for the AverageFilter.
        /// </summary>
        [TestMethod]
        public void AverageFilterTest2()
        {
            AverageFilter af = new AverageFilter(2);
            double[] unfilterdData = new double[] { 310, 31, 33, 432, 35, 777, 77, 30, 27, 246, 263, 300, 84, 958, 27, 37, 32 };
            double[] expectedResult = new double[] { 124.6666666666667, 201.5, 168.2, 261.6, 270.8, 270.2, 189.2, 231.4, 128.6, 173.2, 184, 370.2, 326.4, 281.2, 227.6, 263.5, 32 };
            double[] filterdData = af.Filter(unfilterdData).ToArray();

            for (int i = 0; i < filterdData.Length; i++)
            {
                Assert.AreEqual(expectedResult[i], filterdData[i], 0.0001);
            }
        }
    }
}
