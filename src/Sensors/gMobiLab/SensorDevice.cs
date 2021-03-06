﻿// <summary>
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
using System.Collections.Generic;

using Phrike.Sensors;

namespace Phrike.GMobiLab
{
    /// <summary>
    /// A gMobiLab Sensor device.
    /// </summary>
    public sealed class SensorDevice : ISensorHubDevice
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
        /// Initializes a new instance of the <see cref="SensorDevice"/> class,
        /// referring to the given COM-Port, by default recording no sensors (see
        /// <see cref="SetSensorEnabled"/>).
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
        }

        /// <summary>
        /// Gets an array that includes all channel settings.
        /// It may be indexed: Sensors[i] always corresponds to Channel i + 1.
        /// </summary>
        public IReadOnlyList<SensorInfo> Sensors
        {
            get
            {
                GMobiLabApi.Config cfg;
                if (!GMobiLabApi.GetConfig(device, out cfg))
                {
                    throw new GMobiLabException();
                }

                var result = new SensorInfo[8];
                for (int i = 0; i < result.Length; ++i)
                {
                    result[i] = new SensorInfo(
                        "Channel 0" + (i + 1),
                        Unit.MicroVolt,
                        analogChannelsEnabled[i],
                        i);
                }

                return result;
            }
        }

        /// <summary>
        /// This property is not implemented.
        /// </summary>
        public bool IsUpdating
        {
            get { throw new NotImplementedException(); }
        }

        /// <inheritdoc />
        public int SampleRate { get { return 256; } }

        /// <inheritdoc />
        public string Name => "gMobiLab+";

        /// <inheritdoc/>
        public void Dispose()
        {
            device.Dispose();
        }

        /// <summary>
        /// Set the SD-Card filename or disable the SD-Card.
        /// </summary>
        /// <param name="filename">
        /// The filename on SD-Card or null to disable.
        /// </param>
        /// <remarks>
        ///     The resulting filename on the SD-Card will have the format
        ///     "filename__MP####.BIN" where filename is the given
        ///     <paramref name="filename"/> transformed to upper-case and ####
        ///     is an increasing four-digit number with leading zeroes.
        /// </remarks>
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
        public void StartRecording()
        {
            if (!GMobiLabApi.StartAcquisition(device))
            {
                throw new GMobiLabException();
            }
        }

        /// <summary>
        /// Stops recording the data from the enabled channels on the enabled SDcard
        /// </summary>
        public void StopRecording()
        {
            if (!GMobiLabApi.StopAcquisition(device))
            {
                throw new GMobiLabException();
            }
        }

        /// <summary>
        /// Enables or disables the given <paramref name="sensor"/>.
        /// </summary>
        /// <param name="sensor">
        /// The sensor to enable or disable. Use <see cref="Sensors"/> to
        /// obtain this.
        /// </param>
        /// <param name="enabled">Whether to enable or disable the sensor.</param>
        public void SetSensorEnabled(SensorInfo sensor, bool enabled = true)
        {
            var ain = new GMobiLabApi.AnalogIn();
            ain.ScanChannel = (bool[])analogChannelsEnabled.Clone();

            if (sensor.Id < 0 || sensor.Id >= ain.ScanChannel.Length)
            {
                throw new ArgumentException("This channel doesn't exist.");
            }
            
            ain.ScanChannel[sensor.Id] = enabled;
            if (!GMobiLabApi.InitChannels(device, ain, GMobiLabApi.DigitalIo.Disabled))
            {
                throw new GMobiLabException();
            }

            analogChannelsEnabled[sensor.Id] = enabled;
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        public int GetAvailableSampleCount()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        public int GetSensorValueIndexInSample(SensorInfo sensor)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        public IEnumerable<Sample> ReadSamples(int maxCount = int.MaxValue)
        {
            throw new NotImplementedException();
        }
    }
}
