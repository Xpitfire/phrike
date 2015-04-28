// <summary>
// Implements <see cref="OperationPhrike.GMobiLab.SensorDevice"/>.
// </summary>
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

namespace OperationPhrike.GMobiLab
{
    /// <summary>
    /// The state a <see cref="SensorDevice"/> is currently in.
    /// </summary>
    public enum SensorDeviceState
    {
        /// <summary>The device has been disposed.</summary>
        Disposed,

        /// <summary>
        /// The device has been opened, but transfer has not started yet
        /// or has been stopped.
        /// </summary>
        Openened,

        /// <summary>
        /// The device is transferring data.
        /// </summary>
        Transferring,

        /// <summary>
        /// The device is measuring, but transfer has been paused.
        /// </summary>
        Paused
    }

    /// <summary>
    /// A gMobiLab Sensor device.
    /// </summary>
    public sealed class SensorDevice : ISensorDataSource
    {
        /// <summary>
        /// Sensor device handle.
        /// </summary>
        private readonly GMobiLabApi.Device device;

        /// <summary>
        /// Which analog channels are scanned (0..7).
        /// </summary>
        private bool[] analogChannelsEnabled;

        /// <summary>
        /// The direction or enabledness of the digital channels (0..7).
        /// </summary>
        private DigitalChannelDirection[] digitalChannelDirection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SensorDevice"/> class,
        /// referring to the given COM-Port.
        /// </summary>
        /// <param name="comPort">
        /// The COM port on which the hardware is connected,
        /// in a format like "COM1:".
        /// </param>
        public SensorDevice(string comPort)
        {
            device = GMobiLabApi.OpenDevice(comPort);
            if (device.IsInvalid)
            {
                throw new GMobiLabException();
            }

            // Disable analog channels.
            if (!GMobiLabApi.InitChannels(
                    device,
                    new GMobiLabApi.AnalogIn(),
                    GMobiLabApi.DigitalIo.Disabled))
            {
                throw new GMobiLabException();
            }

            analogChannelsEnabled = new bool[8];
            digitalChannelDirection = Enumerable.Repeat(
                DigitalChannelDirection.Disabled, 8).ToArray();

            State = SensorDeviceState.Openened;
        }

        /// <summary>
        /// Gets the state the device is currently in.
        /// </summary>
        public SensorDeviceState State { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this is a dynamic data source
        /// (always true).
        /// </summary>
        public bool IsDynamic
        {
            get { return true; }
        }

        /// <inheritdoc/>
        public SensorChannel?[] AnalogChannels
        {
            get
            {
                GMobiLabApi.Config cfg;
                if (!GMobiLabApi.GetConfig(device, out cfg))
                {
                    throw new GMobiLabException();
                }

                var result = new SensorChannel?[8];
                for (int i = 0; i < result.Length; ++i)
                {
                    if (analogChannelsEnabled[i])
                    {
                        result[i] = cfg.Channels[i];
                    }
                }

                return result;
            }
        }

        /// <inheritdoc/>
        public DigitalChannelDirection[] DigitalChannels
        {
            get { return digitalChannelDirection; }
        }

        /// <inheritdoc/>
        public int GetAvailableDataCount()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public short[] GetData(int maxCount)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            device.Dispose();
        }

        /// <summary>
        /// Enable or disable the given analog channels.
        /// </summary>
        /// <param name="channels">
        /// The analog channels (0..7) to enable or disable.
        /// </param>
        /// <param name="enabled">
        /// Whether to enable (true) or disable (false) the given channels.
        /// </param>
        public void SetAnalogChannelsEnabled(int[] channels, bool enabled)
        {
            // TODO: Initialize ain.ScanChannel?
            var ain = new GMobiLabApi.AnalogIn();
            ain.ScanChannel = analogChannelsEnabled;

            foreach (var ch in channels)
            {
                if (ch < 0 || ch >= analogChannelsEnabled.Length)
                {
                    throw new ArgumentException(
                        "Channel out of range", "channels");
                }

                ain.ScanChannel[ch] = enabled;
            }

            if (!GMobiLabApi.InitChannels(
                device, ain, GMobiLabApi.DigitalIo.Disabled))
            {
                throw new GMobiLabException();
            }

            foreach (var ch in channels)
            {
                analogChannelsEnabled[ch] = enabled;
            }
        }

        /// <summary>
        /// Set the SD-Card filename or disable the SD-Card.
        /// </summary>
        /// <param name="filename">
        /// The filename on SD-Card or null to disable.
        /// </param>
        public void SetSdFilename(string filename)
        {
            if (!GMobiLabApi.EnableSdCard(device, filename != null))
            {
                throw new GMobiLabException();
            }

            if (
                filename != null
                && !GMobiLabApi.SetFilename(device, filename, filename.Length))
            {
                throw new GMobiLabException();
            }
        }

        /// <summary>
        /// Starts recording the data from the enabled channels on the enabled SDcard
        /// </summary>
        public void StartRecordingData()
        {
            if (!GMobiLabApi.StartAcquisition(device))
            {
                throw new GMobiLabException();
            }
        }

        /// <summary>
        /// Stops recording the data from the enabled channels on the enabled SDcard
        /// </summary>
        public void StopRecordingData()
        {
            if (!GMobiLabApi.StopAcquisition(device))
            {
                throw new GMobiLabException();
            }
        }
    }
}
