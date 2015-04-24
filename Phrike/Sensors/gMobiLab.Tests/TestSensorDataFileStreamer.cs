// <summary>Tests for SensorDataFileStreamer.cs.</summary>
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

namespace OperationPhrike.GMobiLab.Tests
{
    using System.Linq;

    /// <summary>
    /// Tests <see cref="SensorDataFileStreamer"/>.
    /// </summary>
    [TestClass]
    public class TestSensorDataFileStreamer
    {
        /// <summary>
        /// Very rudimentary test whether file headers are parsed correctly.
        /// </summary>
        [TestMethod]
        public void TestHeaderParsing()
        {
            using (var reader = new SensorDataFileStreamer("EKG.bin"))
            {
                Assert.AreEqual(
                    8, reader.AnalogChannels.Where(c => c.HasValue).Count());
            }
        }
    }
}
