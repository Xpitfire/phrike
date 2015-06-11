// <summary>See <see cref="OperationPhrike.GMobiLab.GMobiLabApi"/>.</summary>
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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace OperationPhrike.GMobiLab
{
    /// <summary>
    /// Specifies a polarity for analog channels.
    /// </summary>
    public enum AnalogChannelPolarity : byte
    {
        /// <summary>Channel is unipolar.</summary>
        Unipolar = (byte)'U',

        /// <summary>Channel is bipolar.</summary>
        Bipolar = (byte)'B'
    }

    /// <summary>
    /// Specifies a direction for bidirectional digital channels.
    /// </summary>
    public enum DigitalChannelDirection
    {
        /// <summary>Use digital channel for output.</summary>
        Out = 0, // FALSE

        /// <summary>Use digital channel for input.</summary>
        In = 1, // TRUE

        /// <summary>Don't use digital channel at all.</summary>
        Disabled
    }

    /// <summary>
    /// Known error codes returned by <see cref="GMobiLabApi.GetLastError"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Use <see cref="GMobiLabApi.TranslateErrorCode"/> to retrieve
    /// a matching error message string.
    /// </para>
    /// <para>
    /// These numbers where reverse engineered by calling
    /// <see cref="GMobiLabApi.TranslateErrorCode"/> with
    /// numbers starting from 0.
    /// For all other numbers, this function returned the string
    /// "Number is not a valid error code".
    /// </para>
    /// </remarks>
    public enum GMobiLabErrorCode : uint
    {
        /// <summary>Open communication port failed.</summary>
        OpeningCommunicationPortFailed = 1,

        /// <summary>Configuration of communication port failed.</summary>
        ConfiguringCommunicationPortFailed = 2,

        /// <summary>Configuration of g.MOBIlab+ failed.</summary>
        ConfiguringDeviceFailed = 3,

        /// <summary>Invalid handle value</summary>
        InvalidHandle = 4,

        /// <summary>Communcation with g.MOBIlab+ failed.</summary>
        CommunicationFailed = 5,

        /// <summary>No data available</summary>
        NoDataAvailable = 6,

        /// <summary>Filename not set</summary>
        FilenameNotSet = 7,

        /// <summary>Data transfer from gMOBIlab+ not paused.</summary>
        XferNotPaused = 8,

        /// <summary>Data transfer not resumed.</summary>
        XferNotResumed = 9,

        /// <summary>No SDcard available/SDcard not initialized.</summary>
        NoOrUninitializedSdCard = 10,

        /// <summary>Writing to SDcard not enabled.</summary>
        SdWritingNotEnabled = 11,

        /// <summary>SDcard not disabled.</summary>
        SdNotDisabled = 12,

        /// <summary>Testmode not set.</summary>
        NotInTestmode = 13,

        /// <summary>Device not set to measurement mode.</summary>
        NotInMeasurementMode = 14,

        /// <summary>Device is not streaming.</summary>
        NotStreaming = 15,

        /// <summary>Device did not return status.</summary>
        NoStatusReturned = 16,

        /// <summary>Could not resume device.</summary>
        ResumeFailed = 17,

        /// <summary>Could not set device to pause mode.</summary>
        PauseFailed = 18,

        /// <summary>Could not read device version.</summary>
        ReadingVersionFailed = 19,
    }

    /// <summary>
    /// Holds information about a channel.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SensorChannel
    {
        /// <summary>
        /// Highpass filter.
        /// </summary>
        public float Highpass;

        /// <summary>
        /// Lowpass filter.
        /// </summary>
        public float Lowpass;

        /// <summary>
        /// Channel sensitivity.
        /// </summary>
        public float Sensitivity;

        /// <summary>
        /// Sample rate.
        /// </summary>
        public float SampleRate;

        /// <summary>
        /// Channel polarity.
        /// </summary>
        public AnalogChannelPolarity Polarity;
    }

    /// <summary>
    /// Low level wrapper for gMOBIlabplus.h.
    /// </summary>
    internal static class GMobiLabApi
    {
        /// <summary>
        /// The file name of the gMobiLab DLL, used for DllImport.
        /// </summary>
        private const string GMobiLabDll = "gMOBIlabplus";

        /// <summary>
        /// Open and init the serial interface
        /// </summary>
        /// <param name="port">String containing the serial port e.g."COM1:"</param>
        /// <returns>Whether the call succeeded.</returns>
        [DllImport(GMobiLabDll, EntryPoint = "GT_OpenDevice")]
        public static extern Device OpenDevice(string port);

        /// <summary>
        /// Set the value of digital lines
        /// </summary>
        /// <param name="device">See <see cref="OpenDevice"/>.</param>
        /// <param name="dout">
        /// Bit 7 to bit 4: Set DIO4 to DIO7 (1: set; 0: leave unchanged)
        /// Bit 3 to bit 0: Set values of DIO4 to DIO
        /// </param>
        /// <returns>Whether the call succeeded.</returns>
        [DllImport(GMobiLabDll, EntryPoint = "GT_SetDigitalOut")]
        public static extern bool SetDigitalOut(Device device, byte dout);

        /// <summary>
        /// Init anlog channels and digital lines
        /// </summary>
        /// <param name="device">See <see cref="OpenDevice"/>.</param>
        /// <param name="analogCh">Which analog channels to scan.</param>
        /// <param name="digitalCh">Options for digital channels.</param>
        /// <returns>Whether the call succeeded.</returns>
        [DllImport(GMobiLabDll, EntryPoint = "GT_InitChannels")]
        public static extern bool InitChannels(
            Device device, AnalogIn analogCh, DigitalIo digitalCh);

        /// <summary>
        /// Start Acqusition of selected channels and lines
        /// (see <see cref="InitChannels"/>).
        /// </summary>
        /// <param name="device">See <see cref="OpenDevice"/>.</param>
        /// <returns>Whether the call succeeded.</returns>
        [DllImport(GMobiLabDll, EntryPoint = "GT_StartAcquisition")]
        public static extern bool StartAcquisition(Device device);

        /// <summary>
        /// Stop acqusition (see <see cref="StartAcquisition"/>).
        /// (see <see cref="InitChannels"/>).
        /// </summary>
        /// <param name="device">See <see cref="OpenDevice"/>.</param>
        /// <returns>Whether the call succeeded.</returns>
        [DllImport(GMobiLabDll, EntryPoint = "GT_StopAcquisition")]
        public static extern bool StopAcquisition(Device device);

        // TODO: Import GT_GetData (needs struct _BUFFER_ST)

        /// <summary>
        /// Read the configuration from g.MOBIlab+
        /// </summary>
        /// <param name="device">See <see cref="OpenDevice"/>.</param>
        /// <param name="cfg">Receives the read configuration.</param>
        /// <returns>Whether the call succeeded.</returns>
        [DllImport(GMobiLabDll, EntryPoint = "GT_GetConfig")]
        public static extern bool GetConfig(Device device, out Config cfg);

        /// <summary>
        /// Set the filename used for the file stored on the SDcard
        /// </summary>
        /// <param name="device">See <see cref="OpenDevice"/>.</param>
        /// <param name="filename">String containing the filename.</param>
        /// <param name="fnameLength">Length of the filename string.</param>
        /// <returns>Whether the call succeeded.</returns>
        [DllImport(GMobiLabDll, EntryPoint = "GT_SetFilename")]
        public static extern bool SetFilename(
            Device device, string filename, int fnameLength);

        /// <summary>
        /// Pause the data transmossion from g.MOBIlab+ to PC.
        /// </summary>
        /// <remarks>Device continues to stream data to SDcard.</remarks>
        /// <param name="device">See <see cref="OpenDevice"/>.</param>
        /// <returns>Whether the call succeeded.</returns>
        [DllImport(GMobiLabDll, EntryPoint = "GT_PauseXfer")]
        public static extern bool PauseXfer(Device device);

        /// <summary>
        /// Resume the data transfer from g.MOBIlab+ to PC..
        /// </summary>
        /// <remarks>
        /// Device restarts to send data via Bluethoot
        /// or serial connection to the PC.
        /// </remarks>
        /// <param name="device">See <see cref="OpenDevice"/>.</param>
        /// <returns>Whether the call succeeded.</returns>
        [DllImport(GMobiLabDll, EntryPoint = "GT_ResumeXfer")]
        public static extern bool ResumeXfer(Device device);

        /// <summary>
        /// Enable the SDcard if inserted into g.MOBIlab+.
        /// </summary>
        /// <param name="device">See <see cref="OpenDevice"/>.</param>
        /// <param name="enable">
        /// Whether to enable or disable the SDcard.
        /// </param>
        /// <returns>Whether the call succeeded.</returns>
        [DllImport(GMobiLabDll, EntryPoint = "GT_EnableSDcard")]
        public static extern bool EnableSdCard(Device device, bool enable);

        // GT_GetDeviceStatus not exposed because it always fails.

        /// <summary>
        /// Read the remaining size of the SDcard.
        /// </summary>
        /// <param name="device">See <see cref="OpenDevice"/>.</param>
        /// <param name="freeSize">
        /// Receives the remaining size in Bytes.
        /// <para>
        /// If no SDcard is inserted size = 0,
        /// if size less than 2MB device will not start to stream.
        /// </para>
        /// </param>
        /// <returns>Whether the call succeeded.</returns>
        [DllImport(GMobiLabDll, EntryPoint = "GT_GetSDcardStatus")]
        public static extern bool GetSdCardFreeSize(
            Device device, out uint freeSize);

        /// <summary>
        /// Set the device to testmode to check connection to PC.
        /// </summary>
        /// <param name="device">See <see cref="OpenDevice"/>.</param>
        /// <param name="enable">
        /// Whether to enable or disable the testmode.
        /// </param>
        /// <returns>Whether the call succeeded.</returns>
        [DllImport(GMobiLabDll, EntryPoint = "GT_SetTestmode")]
        public static extern bool EnableTestmode(Device device, bool enable);

        /// <summary>
        /// Get the driver version of g.MOBIlab+.
        /// </summary>
        /// <returns>
        /// Driver version (minor version as fractional digits).
        /// </returns>
        [DllImport(GMobiLabDll, EntryPoint = "GT_GetDriverVersion")]
        public static extern float GetDriverVersion();

        /// <summary>Retrieve Error Code from driver.</summary>
        /// <param name="lastError">Last error code.</param>
        /// <returns>Whether the call succeeded.</returns>
        [DllImport(GMobiLabDll, EntryPoint = "GT_GetLastError")]
        public static extern bool GetLastError(out GMobiLabErrorCode lastError);

        /// <summary>Retrieve Error String for specified error.</summary>
        /// <param name="errorString">
        /// Receives the error message corresponding to
        /// <paramref name="errorCode"/>.
        /// </param>
        /// <param name="errorCode">
        /// Error code as retrieved by <see cref="GetLastError"/>.
        /// </param>
        /// <returns>Whether the call succeeded.</returns>
        [DllImport(GMobiLabDll, EntryPoint = "GT_TranslateErrorCode")]
        public static extern bool TranslateErrorCode(
            out ErrorString errorString, GMobiLabErrorCode errorCode);

        /// <summary>
        /// Close the serial interface
        /// </summary>
        /// <remarks>
        /// Only used by the nested <see cref="Device"/> class.
        /// </remarks>
        /// <param name="hDevice">See <see cref="OpenDevice"/>.</param>
        /// <returns>Whether the call succeeded.</returns>
        [DllImport(GMobiLabDll, EntryPoint = "GT_CloseDevice")]
        private static extern bool CloseDevice(IntPtr hDevice);

        /// <summary>
        /// Specifies which analog input channels to enable.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct AnalogIn
        {
            /// <summary>
            /// A AnalogIn with all channels disabled and set to input.
            /// </summary>
            public static readonly AnalogIn Disabled = new AnalogIn
            {
                ScanChannel = new bool[8]
            };

            /// <summary>Whether to scan analog channels 1..8.</summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public bool[] ScanChannel;
        }

        /// <summary>
        /// Specifies options for digital channels.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct DigitalIo
        {
            /// <summary>
            /// A DigitalIo with all channels disabled and set to input.
            /// </summary>
            public static readonly DigitalIo Disabled = new DigitalIo
            {
                Channel4Direction = DigitalChannelDirection.In,
                Channel5Direction = DigitalChannelDirection.In,
                Channel6Direction = DigitalChannelDirection.In,
                Channel7Direction = DigitalChannelDirection.In,
                ScanChannel = new bool[8]
            };

            /// <summary>Whether to scan digital channels 1..8.</summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public bool[] ScanChannel;

            /// <summary>
            /// Whether to use digital channel 4 for input or output. 
            /// </summary>
            public DigitalChannelDirection Channel4Direction;

            /// <summary>
            /// Whether to use digital channel 5 for input or output. 
            /// </summary>
            public DigitalChannelDirection Channel5Direction;

            /// <summary>
            /// Whether to use digital channel 6 for input or output. 
            /// </summary>
            public DigitalChannelDirection Channel6Direction;

            /// <summary>
            /// Whether to use digital channel 7 for input or output. 
            /// </summary>
            public DigitalChannelDirection Channel7Direction;
        }

        /// <summary>
        /// Used to retrieve error strings.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct ErrorString
        {
            /// <summary>
            /// The error message.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Error;
        }

        /// <summary>
        /// Specifies options for digital channels.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct Config
        {
            /// <summary>
            /// Device (?) version.
            /// </summary>
            public short Version;

            /// <summary>
            /// Device serial number.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            public string Serial;

            /// <summary>
            /// Channel information.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public SensorChannel[] Channels;
        }

        /// <summary>
        /// Wraps a gMobiLab device HANDLE
        /// (see <see cref="GMobiLabApi.OpenDevice"/>).
        /// </summary>
        public class Device : SafeHandleZeroOrMinusOneIsInvalid
        {
            /// <summary>
            /// Constructor; Only called by P/Invoke functionality.
            /// </summary>
            [SuppressMessage(
                "StyleCop.CSharp.DocumentationRules",
                "SA1642:ConstructorSummaryDocumentationMustBeginWithStandardText",
                Justification =
                    "Requested standard text would be misleading here.")]
            private Device()
                : base(ownsHandle: true)
            {
            }

            /// <inheritdoc/>
            protected override bool ReleaseHandle()
            {
                return GMobiLabApi.CloseDevice(handle);
            }
        }
    }
}
