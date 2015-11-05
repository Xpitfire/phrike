// <summary>Tests for DataBundle.</summary>
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

using Phrike.Sensors;

namespace Sensors.Test
{

    [TestClass]
    public class DataBundleTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var bundle = new DataBundle();
            Assert.IsNotNull(bundle.DataSeries);
            Assert.AreEqual(0, bundle.DataSeries.Count);
        }

        [TestMethod]
        public void FromEmptyHubTest()
        {
            var hub = new FixedSampleSensorHub()
            {
                Name = "HubName",
                SampleRate = 100,
                WriteableSensors = new SensorInfo[0],
                Samples = new Sample[0]
            };

            DataBundle bundle = DataBundle.FromHub(hub);
            Assert.AreEqual(0, bundle.DataSeries.Count);
        }

        [TestMethod]
        public void FromSingleSeriesHubTest()
        {
            var sampleValues = new[] {
                new [] {0.0},
                new [] {1.0},
                new [] {2.0}
            };
            var hub = new FixedSampleSensorHub()
            {
                Name = "HubName",
                SampleRate = 100,
                WriteableSensors = new []
                {
                    new SensorInfo("SensA", Unit.Bpm, true, 0)
                },
                Samples = new []
                {
                    new Sample(sampleValues[0]), 
                    new Sample(sampleValues[1]), 
                    new Sample(sampleValues[2]), 
                }
            };

            DataBundle bundle = DataBundle.FromHub(hub);
            Assert.AreEqual(1, bundle.DataSeries.Count);
            DataSeries series = bundle.DataSeries[0];
            Assert.AreEqual(Unit.Bpm, series.Unit);
            Assert.AreEqual("HubName", series.SourceName);
            Assert.AreEqual("SensA", series.Name);
            CollectionAssert.AreEqual(
                sampleValues.Select(a => a[0]).ToArray(),
                series.Data);
        }

        [TestMethod]
        public void FromMultiSeriesHubTest()
        {
            var sampleValues = new[] {
                new [] {0.0, 0.1},
                new [] {1.0, 1.1},
                new [] {2.0, 2.1}
            };

            var hub = new FixedSampleSensorHub()
            {
                Name = "HubName",
                SampleRate = 100,
                WriteableSensors = new []
                {
                    new SensorInfo("SensA", Unit.Bpm, true, 0),
                    new SensorInfo("SensB", Unit.DegreeCelsius, true, 1)
                },
                Samples = new []
                {
                    new Sample(sampleValues[0]), 
                    new Sample(sampleValues[1]), 
                    new Sample(sampleValues[2])
                }
            };

            DataBundle bundle = DataBundle.FromHub(hub);
            Assert.AreEqual(2, bundle.DataSeries.Count);
            DataSeries series0 = bundle.DataSeries[0];
            Assert.AreEqual("HubName", series0.SourceName);
            Assert.AreEqual("SensA", series0.Name);
            Assert.AreEqual(series0.Unit, Unit.Bpm);
            CollectionAssert.AreEqual(
                series0.Data,
                sampleValues.Select(a => a[0]).ToArray());
            DataSeries series1 = bundle.DataSeries[1];
            Assert.AreEqual("HubName", series1.SourceName);
            Assert.AreEqual("SensB", series1.Name);
            Assert.AreEqual(series1.Unit, Unit.DegreeCelsius);
            CollectionAssert.AreEqual(
                series1.Data,
                sampleValues.Select(a => a[1]).ToArray());
        }

        [TestMethod]
        public void FromMultiSeriesHubWithDisabledSensorTest()
        {
            var sampleValues = new[] {
                new [] {0.0, 0.1},
                new [] {1.0, 1.1},
                new [] {2.0, 2.1}
            };

            var hub = new FixedSampleSensorHub()
            {
                Name = "HubName",
                SampleRate = 100,
                WriteableSensors = new []
                {
                    new SensorInfo("SensA", Unit.Bpm, true, 0),
                    new SensorInfo("SensX", Unit.Unknown, false, 1),
                    new SensorInfo("SensB", Unit.DegreeCelsius, true, 2)
                },
                Samples = new []
                {
                    new Sample(sampleValues[0]), 
                    new Sample(sampleValues[1]), 
                    new Sample(sampleValues[2])
                }
            };

            DataBundle bundle = DataBundle.FromHub(hub);
            Assert.AreEqual(2, bundle.DataSeries.Count);
            DataSeries series0 = bundle.DataSeries[0];
            Assert.AreEqual("HubName", series0.SourceName);
            Assert.AreEqual("SensA", series0.Name);
            Assert.AreEqual(series0.Unit, Unit.Bpm);
            CollectionAssert.AreEqual(
                series0.Data,
                sampleValues.Select(a => a[0]).ToArray());
            DataSeries series1 = bundle.DataSeries[1];
            Assert.AreEqual("HubName", series1.SourceName);
            Assert.AreEqual("SensB", series1.Name);
            Assert.AreEqual(series1.Unit, Unit.DegreeCelsius);
            CollectionAssert.AreEqual(
                series1.Data,
                sampleValues.Select(a => a[1]).ToArray());
        }
    }
}