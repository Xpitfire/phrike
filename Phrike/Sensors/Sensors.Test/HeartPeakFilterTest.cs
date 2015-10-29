// <summary></summary>
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

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sensors.Test
{
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The heart peak filter test.
    /// </summary>
    [TestClass]
    public class HeartPeakFilterTest
    {
        /// <summary>
        /// The max peaks.
        /// </summary>
        private double[] maxPeaks = new double[] { 0, 0, 5, 0, 0, 0, 3, 0, 0, 0, 0, 0, 3, 0, 0 };

        /// <summary>
        /// The min peaks.
        /// </summary>
        private double[] minPeaks = new double[] { 0, 0, 4, 0, 0, 0, 0, 2, 0, 0, 0, 4, 0, 0, 0 };

        /// <summary>
        /// The result.
        /// </summary>
        private double[] expectedResult   = new double[] { 0, 0, 9, 0, 0, 0, 5, 0, 0, 0, 0, 0, 7, 0, 0 };

        /// <summary>
        /// The max peak distance.
        /// </summary>
        private int maxPeakDistance = 2;

        /// <summary>
        /// The merge peaks test.
        /// </summary>
        [TestMethod]
        public void MergePeaksTest()
        {
            //var result = new 
        }
    }
}