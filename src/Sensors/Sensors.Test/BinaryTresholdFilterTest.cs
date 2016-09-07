// <summary> Unit-Test for BinaryTresholdFilter</summary>
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
    /// Class to test the BinaryTresholdFilter.
    /// </summary>
    [TestClass]
    public class BinaryTresholdFilterTest
    {
        /// <summary>
        /// Test for the BinaryTresholdFilter. 
        /// </summary>
        [TestMethod]
        public void BinaryTresholdFilterTest1()
        {
            BinaryThresholdFilter btf = new BinaryThresholdFilter(3);
            double[] unfilterdData = new double[] { 84, 958, 27, 310, 31, 33, 77, 30, 27, 246, 263, 300, 432, 35, 777, 37, 32 };
            double[] expectedResult = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            double[] filterdData = btf.Filter(unfilterdData).ToArray();

            for (int i = 0; i < filterdData.Length; i++)
            { 
                Assert.AreEqual(expectedResult[i], filterdData[i]);
            }

            CollectionAssert.AreEqual(expectedResult, filterdData);
        }

        /// <summary>
        /// Test for the BinaryTresholdFilter with negative values. 
        /// </summary>
        [TestMethod]
        public void BinaryTresholdFilterTest2()
        {
            BinaryThresholdFilter btf = new BinaryThresholdFilter(3);
            double[] unfilterdData = new double[] { 0, -1, 2, -3, 4, 5, 6, -7 };
            double[] expectedResult = new double[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            double[] filterdData = btf.Filter(unfilterdData).ToArray();

            for (int i = 0; i < filterdData.Length; i++)
            {
                Assert.AreEqual(expectedResult[i], filterdData[i]);
            }

            CollectionAssert.AreEqual(expectedResult, filterdData);
        }
    }
}
