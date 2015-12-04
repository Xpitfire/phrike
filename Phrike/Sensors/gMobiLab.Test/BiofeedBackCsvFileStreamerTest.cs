// <summary> Unit-Test for BiofeedBackCsvFileStreamer</summary>
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
using Phrike.Sensors;

namespace gMobiLab.Test
{
    /// <summary>
    /// Class to test the BiofeedBackFileStreamer.
    /// </summary>
    [TestClass]
    public class BioFeedBackCsvFileStreamerTest
    {
        /// <summary>
        /// Test for the first values in the sample.
        /// </summary>
        [TestMethod]
        public void BioFeedBackCsvFileStreamerTest1()
        {
            using (BiofeedbackCsvFileStreamer b = new BiofeedbackCsvFileStreamer("2015TdoT5.csv"))
            {
                Sample[] fileValues = b.ReadSamples().ToArray();
                double[] expectedValues = new double[]
                                          {
                                              10.129426, 0, 0, 9.356974602, 26.53000069, 45.56776428, 20.29304123
                                          };

                for (int j = 0; j < 7; j++)
                {
                    Assert.AreEqual(expectedValues[j], fileValues[0].SensorValues[j], 0.00000000001);
                }
            }
        }

        /// <summary>
        /// Test for the last values in the sample.
        /// </summary>
        [TestMethod]
        public void BioFeedBackCsvFileStreamerTest2()
        {
            using (BiofeedbackCsvFileStreamer b = new BiofeedbackCsvFileStreamer("2015TdoT5.csv"))
            {
                double[] expectedValues = new double[]
                                        {
                                            10.11477375, 0.092796095, 0.012278579,
                                            12.90780354, 28.73999977, 51.30647278, 32.72283173
                                        };
                Sample[] fileValues = b.ReadSamples().ToArray();
              
                for (int j = 0; j < 7; j++)
                {
                    Console.WriteLine("{0}", fileValues[fileValues.Length - 1].SensorValues[j]);
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
            using (BiofeedbackCsvFileStreamer b = new BiofeedbackCsvFileStreamer("2015TdoT5.csv"))
            {
                Assert.AreEqual(9, b.Sensors.Count);             
            }
        }

        /// <summary>
        /// Test the enabled sensors.
        /// </summary>
        [TestMethod]
        public void SensorDataFileStreamerTest4()
        {
            using (BiofeedbackCsvFileStreamer b = new BiofeedbackCsvFileStreamer("2015TdoT5.csv"))
            {
                var sensorInfos = b.Sensors;
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
            using (BiofeedbackCsvFileStreamer b = new BiofeedbackCsvFileStreamer("2015TdoT5.csv"))
            {
                var sensorInfos = b.Sensors;
                string[] names = new string[]
                                 {
                                  "R_20_Resp1", "R_21_RespA1", "R_22_RespF1", 
                                  "U_13_SCL", "U_15_Temp", "U_16_BVP", "U_17_PVA",
                                  "U_18_Puls", "U_19_Mot"
                                 };
                for (int i = 0; i < sensorInfos.Count; i++)
                {
                    Assert.AreEqual(names[i], sensorInfos[i].Name);
                }
            }
        }
    }
}
