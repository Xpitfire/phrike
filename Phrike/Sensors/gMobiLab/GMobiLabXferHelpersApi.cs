// <summary>See <see cref="OperationPhrike.GMobiLab.GMobiLabXferHelpersApi"/>.</summary>
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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OperationPhrike.GMobiLab
{
    /// <summary>
    /// Low level wrapper for the gMobiLabXferHelpers DLL.
    /// </summary>
    internal static class GMobiLabXferThreadApi
    {
        /// <summary>
        /// A thin wrapper that makes destroying Xfer thread easier.
        /// </summary>
        public class XferThread: IDisposable
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="XferThread"/>
            /// class with the given device.
            /// </summary>
            /// <param name="device">
            /// The device from which the thread should extract data.
            /// </param>
            public XferThread(GMobiLabApi.Device device)
            {
                Value = GMobiLabXferThreadApi.Create(device);
                if (Value == IntPtr.Zero)
                {
                    throw new GMobiLabXferException(
                        "Failed creating Xfer thread.");
                }
            }

            /// <summary>
            /// Destroys the Xfer thread, if <see cref="Dispose"/>
            /// has not already been called.
            /// </summary>
            ~XferThread()
            {
                Dispose();
            }

            /// <summary>
            /// Gets the held thread pointer.
            /// </summary>
            public IntPtr Value { get; private set; }

            /// <summary>
            /// Calls <see cref="Destroy"/> on the wrapped Xfer thread.
            /// </summary>
            public void Dispose()
            {
                GMobiLabXferThreadApi.Destroy(Value);
                Value = IntPtr.Zero;
            }
        }

        /// <summary>
        /// The file name of the gMobiLabXferHelpers DLL, used for DllImport.
        /// </summary>
        private const string XferHelpersDll = "gMobiLabXferHelpers";

        /// <summary>
        /// Creates a new Xfer thread but does not start it.
        /// You must free the thread with <see cref="Destroy"/>.
        /// </summary>
        /// <param name="device">
        /// See <see cref="GMobiLabApi.OpenDevice"/>.
        /// </param>
        /// <returns>
        /// A pointer to a new Xfer thread or IntPtr.Zero on failure.
        /// </returns>
        [DllImport(XferHelpersDll, EntryPoint = "Phrike_CreateXferThread")]
        public static extern IntPtr Create(GMobiLabApi.Device device);

        /// <summary>
        /// Starts the given Xfer thread.
        /// </summary>
        /// <remarks>
        /// <see cref="GMobiLabApi.StartAcquisition"/> must have been called on
        /// the corresponding device handle, or the thread will stop
        /// immediately.
        /// </remarks>
        /// <param name="xfer">See <see cref="Create"/>.</param>
        /// <returns>Whether the call succeeded.</returns>
        [DllImport(XferHelpersDll, EntryPoint = "Phrike_StartXferThread")]
        public static extern bool Start(IntPtr xfer);

        /// <summary>
        /// Destroys the given Xfer thread if possible.
        /// </summary>
        /// <remarks>
        /// This function fails if the thread is still running. Call
        /// <see cref="GMobiLabApi.StopAcquisition"/>, drop or consume
        /// all remaining data and wait for the thread to exit before
        /// calling this function.
        /// </remarks>
        /// <param name="xfer">See <see cref="Create"/>.</param>
        /// <returns>Whether the call succeeded.</returns>
        [DllImport(XferHelpersDll, EntryPoint = "Phrike_DestroyXferThread")]
        public static extern bool Destroy(IntPtr xfer);

        /// <summary>
        /// Wait for the given Xfer thread to exit. See <see cref="Destroy"/>.
        /// </summary>
        /// <param name="xfer">See <see cref="Create"/>.</param>
        /// <returns>Whether the call succeeded.</returns>
        [DllImport(XferHelpersDll, EntryPoint = "Phrike_JoinXferThread")]
        public static extern bool Join(IntPtr xfer);

        /// <summary>
        /// Drop all data from an xfer thread. Useful before destroying.
        /// </summary>
        /// <param name="xfer">See <see cref="Create"/>.</param>
        /// <returns>How many shorts were dropped.</returns>
        [DllImport(XferHelpersDll, EntryPoint = "Phrike_DropXferData")]
        public static extern uint DropData(IntPtr xfer);

        /// <summary>
        /// Get the data collected so far by the Xfer thread.
        /// </summary>
        /// <param name="xfer">See <see cref="Create"/>.</param>
        /// <param name="count">
        /// Set to the maximum number of shorts to copy to data.
        /// Upon successful return from this function, receives
        /// the number of shorts actually copied (can be less).
        /// </param>
        /// <param name="data">
        /// Pointer to native array of at least <paramref name="count"/> shorts.
        /// </param>
        /// <returns>Whether the call succeeded.</returns>
        [DllImport(XferHelpersDll, EntryPoint = "Phrike_GetXferData")]
        public static extern bool GetData(
            IntPtr xfer, ref uint count, IntPtr data);

        /// <summary>
        /// Returns how many shorts have been collected so far by xfer.
        /// </summary>
        /// <remarks>
        /// Note that the actual value can increase between the call to this
        /// function and a call to <see cref="GetData"/>.
        /// </remarks>
        /// <param name="xfer">See <see cref="Create"/>.</param>
        /// <returns>How many shorts have been collected so far.</returns>
        [DllImport(XferHelpersDll, EntryPoint = "Phrike_AvailableXferCount")]
        public static extern uint AvailableCount(IntPtr xfer);
    }
}
