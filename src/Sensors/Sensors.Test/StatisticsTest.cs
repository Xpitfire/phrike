// <summary>Tests for Statistics.</summary>
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phrike.Sensors;
using System.Linq;

namespace Sensors.Test
{

    [TestClass]
    public class StatisticsTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var stats = new Statistics();
            Assert.AreEqual(stats.Average, 0);
            Assert.AreEqual(stats.Slope, 0);
            Assert.AreEqual(stats.Intercept, 0);
            Assert.AreEqual(stats.DeterminationCoefficient, 0);
            Assert.AreEqual(stats.Max, 0);
            Assert.AreEqual(stats.Min, 0);
            Assert.AreEqual(stats.Variance, 0);
        }

        [TestMethod]
        public void FromDataSeriesTest()
        {
            var arr = new double[] {1, 2, 3, 4};
            var series = new DataSeries(arr, 0, null, null, Unit.Unknown);
            Statistics stats = Statistics.FromDataSeries(series);
            Assert.AreEqual(stats.Average, arr.Average());
            Assert.AreEqual(stats.Slope, arr.Slope());
            Assert.AreEqual(stats.Intercept, arr.Intercept());
            Assert.AreEqual(stats.DeterminationCoefficient, arr.DeterminationCoefficient());
            Assert.AreEqual(stats.Min, arr.Min());
            Assert.AreEqual(stats.Max, arr.Max());
        }
    }
}