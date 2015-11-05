// <summary>Tests for DataSeries.</summary>
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

namespace Sensors.Test
{
    [TestClass]
    public class DataSeriesTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var arr = new double[] { 1, 2, 3 };
            var series = new DataSeries(arr, 42, "MySource", "MyName", Unit.DegreeCelsius);
            Assert.AreSame(arr, series.Data);
            Assert.AreEqual(42, series.SampleRate);
            Assert.AreEqual("MySource", series.SourceName);
            Assert.AreEqual("MyName", series.Name);
            Assert.AreEqual(series.Unit, Unit.DegreeCelsius);
            Assert.IsNotNull(series.Statistics);
        }

        [TestMethod]
        public void StatisticsTest()
        {
            var arr = new double[] { 1, 2, 3 };
            var series = new DataSeries(arr, 42, null, null, Unit.Unknown);
            Statistics stat1 = Statistics.FromDataSeries(series);
            Statistics stat2 = series.Statistics;
            Assert.AreNotSame(stat1, stat2);
            Assert.AreEqual(stat1.Average, stat2.Average);
            Assert.AreEqual(stat1.DeterminationCoefficient, stat2.DeterminationCoefficient);
            Assert.AreEqual(stat1.Intercept, stat2.Intercept);
            Assert.AreEqual(stat1.Max, stat2.Max);
            Assert.AreEqual(stat1.Slope, stat2.Slope);
            Assert.AreEqual(stat1.Variance, stat2.Variance);
        }
    }
}