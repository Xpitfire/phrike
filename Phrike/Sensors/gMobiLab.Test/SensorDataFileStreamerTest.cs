// <summary> Unit-Test for SensorDataFileStreamer</summary>
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
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phrike.GMobiLab;
using Phrike.Sensors;

namespace gMobiLab.Test
{
    /// <summary>
    /// Class to test the SensorDataFileStreamer.
    /// </summary>
    [TestClass]
    public class SensorDataFileStreamerTest
    {
        /// <summary>
        /// Test for the first values in the sample.
        /// </summary>
        [TestMethod]
        public void SensorDataFileStreamerTest1()
        {
            using (SensorDataFileStreamer s = new SensorDataFileStreamer("test.bin.testin"))
            {
                double[] expectedValues = new double[]
                                          {
                                              -3.997802734375E-05, 0.00016693115234375, 0.001495361328125,
                                              0.002322998046875, -0.0046234130859375, -0.0025543212890625,
                                              -0.024261474609375
                                          };
                Sample[] fileValues = s.ReadSamples().ToArray();
                for (int j = 0; j < 7; j++)
                {
                    Console.WriteLine("{0} : {1}", expectedValues[j], fileValues[0].SensorValues[j]);
                    Assert.AreEqual(expectedValues[j], fileValues[0].SensorValues[j], 0.00000000001);
                }
            }
        }

        /// <summary>
        /// Test for the last values in the sample.
        /// </summary>
        [TestMethod]
        public void SensorDataFileStreamerTest2()
        {
            using (SensorDataFileStreamer s = new SensorDataFileStreamer("test.bin.testin"))
            {
                double[] expectedValues = new double[]
                                          {
                                              0.000174674987792969, 0.000571174621582031, -0.00112930297851562,
                                              0.000456695556640625, 0.00510673522949219, -0.00342826843261719,
                                              0.0268363952636719
                                          };

                Sample[] fileValues = s.ReadSamples().ToArray();

                for (int j = 0; j < 7; j++)
                {
                    Console.WriteLine("{0} : {1}", expectedValues[j], fileValues[fileValues.Length - 1].SensorValues[j]);
                    Assert.AreEqual(expectedValues[j], fileValues[fileValues.Length - 1].SensorValues[j], 0.00000000001);
                }
            }
        }

        /// <summary>
        /// Test for amount of sensors.
        /// </summary>
        [TestMethod]
        public void SensorDataFileStreamerTest3()
        {
            using (SensorDataFileStreamer s = new SensorDataFileStreamer("test.bin.testin"))
            {
                Assert.AreEqual(8, s.Sensors.Count);
            }
        }

        /// <summary>
        /// Test the enabled sensors.
        /// </summary>
        [TestMethod]
        public void SensorDataFileStreamerTest4()
        {
            using (SensorDataFileStreamer s = new SensorDataFileStreamer("test.bin.testin"))
            {
                var sensorInfos = s.Sensors;
                foreach (var sensorInfo in sensorInfos)
                {
                    Assert.AreEqual(true, sensorInfo.Enabled);
                }
            }
        }

        /// <summary>
        /// Test the names of the sensors. 
        /// </summary>
        [TestMethod]
        public void SensorDataFileStreamerTest5()
        {
            using (SensorDataFileStreamer s = new SensorDataFileStreamer("test.bin.testin"))
            {
                var sensorInfos = s.Sensors;
                string[] names = new string[]
                                 {
                                     "Channel 01", "Channel 02", "Channel 03", "Channel 04", "Channel 05", "Channel 06",
                                     "Channel 07", "Channel 08"
                                 };
                for (int i = 0; i < sensorInfos.Count; i++)
                {
                    Assert.AreEqual(names[i], sensorInfos[i].Name);
                }
            }
        }
    }
}
