// <summary>Unit-Test for PulseCalculator</summary>
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
namespace Sensors.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Phrike.Sensors.Filters;

    /// <summary>
    /// The pulse calculator test.
    /// </summary>
    [TestClass]
    public class PulseCalculatorTest
    {
        /// <summary>
        /// The peaks.
        /// </summary>
        private readonly double[] peaks = new double[] { 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1 };

        /// <summary>
        /// The heart rate.
        /// </summary>
        private readonly double[] heartRate = new double[] 
            {
                34.28, 34.28, 34.28, 34.28, 34.28, 34.28, 34.28, 34.28,
                40, 40, 40, 40, 40, 40, 40, 40, 40, 40,
                34.28, 34.28, 34.28, 34.28, 34.28, 34.28, 34.28,
                40, 40, 40, 40, 40, 40
            };

        /// <summary>
        /// Test the values of the PcFilter
        /// </summary>
        [TestMethod]
        public void PcFilterTest()
        {
            PulseCalculator pulseCalculator = new PulseCalculator(null, 4);

            var heartRateReal = pulseCalculator.Filter(peaks);
            for (int i = 0; i < heartRateReal.Count; i++)
            {
                Assert.AreEqual(heartRate[i], heartRateReal[i], 0.01);
            }
        }
    }
}