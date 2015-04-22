// <summary>Tests for GMobiLabApi.cs.</summary>
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

namespace OperationPhrike.GMobiLab.Tests
{
    using System.Diagnostics;

    /// <summary>
    /// Test the GMobiLabApi low level class.
    /// </summary>
    [TestClass]
    public class TestGMobiLabApi
    {
        /// <summary>
        /// Test that OpenDevice fails with a bad name.
        /// </summary>
        [TestMethod]
        public void TestFailedOpen()
        {
            using (var dev = GMobiLabApi.OpenDevice("NOPE:"))
            {
                Assert.IsTrue(dev.IsInvalid);
                GMobiLabErrorCode err;
                Assert.IsTrue(GMobiLabApi.GetLastError(out err));
                Assert.AreEqual(
                    GMobiLabErrorCode.OpeningCommunicationPortFailed, err);
            }
        }
    }
}
