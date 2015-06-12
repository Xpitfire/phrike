// <summary>Implements SensorDataFileStreamer.</summary>
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using Phrike.Sensors;

namespace Phrike.GMobiLab
{
    /// <summary>
    ///     Reads a data file as written to the SDCard by the sensor device.
    /// </summary>
    public sealed class SensorDataFileStreamer : ISensorHub
    {
        /// <summary>
        ///     Reader for the binary data in <see cref="file" />.
        /// </summary>
        /// <remarks>
        ///     Is not disposed because disposing the underlying file is enough.
        /// </remarks>
        private readonly BinaryReader dataReader;

        /// <summary>
        ///     The underlying data file.
        /// </summary>
        private readonly FileStream file;

        /// <summary>
        /// Saves information about the analog channels.
        /// </summary>
        private readonly SensorChannel?[] analogChannels;

        /// <summary>
        /// The approximate start time of the recording.
        /// </summary>
        private readonly DateTime startTime;

        /// <summary>
        /// Information about the sensors.
        /// </summary>
        private readonly SensorInfo[] sensorInfos;

        /// <summary>
        /// Saves the number of channels in the file (digital sensors are
        /// bundled in one channel if enabled).
        /// </summary>
        private int recordedChannelCount;

        /// <summary>
        /// The samplerate in Hz.
        /// </summary>
        private int sampleRate;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SensorDataFileStreamer"/> class.
        /// </summary>
        /// <param name="filename">
        /// Path to an existing sensor binary file.
        /// </param>
        public SensorDataFileStreamer(string filename)
        {
            DateTime creationTime = File.GetCreationTime(filename);
            DateTime lastWriteTime = File.GetLastWriteTime(filename);
            startTime = creationTime < lastWriteTime ?
                creationTime : lastWriteTime;

            file = new FileStream(filename, FileMode.Open);
            dataReader = new BinaryReader(file);
            this.analogChannels = new SensorChannel?[8];

            ParseHeader();

            sensorInfos = new SensorInfo[this.analogChannels.Length];
            for (int i = 0; i < sensorInfos.Length; ++i)
            {
                sensorInfos[i] = new SensorInfo(
                    "Channel 0" + (i + 1).ToString(),
                    Unit.MicroVolt,
                    this.analogChannels[i].HasValue,
                    i);
            }
        }

        /// <summary>
        /// Gets information about the available sensors/channels. 
        /// It may be indexed: Sensors[i] always corresponds to Channel i + 1.
        /// </summary>
        public IReadOnlyList<SensorInfo> Sensors
        {
            get
            {
                return sensorInfos;
            }
        }

        /// <summary>
        /// Gets a value indicating whether new samples may become available.
        /// Always false.
        /// </summary>
        public bool IsUpdating
        {
            get { return false; }
        }

        /// <inheritdoc/>
        public void SetSensorEnabled(SensorInfo sensor, bool enabled = true)
        {
            if (sensor.Id < 0 || sensor.Id >= analogChannels.Length)
            {
                throw new ArgumentException("Sensor ID out of range.", "sensor");
            }

            if (!analogChannels[sensor.Id].HasValue)
            {
                throw new InvalidOperationException(
                    "Cannot enable a sensor that was not recorded.");
            }

            SensorInfo oldInfo = sensorInfos[sensor.Id];
            sensorInfos[sensor.Id] = new SensorInfo(
                oldInfo.Name, oldInfo.Unit, enabled, oldInfo.Id);
        }

        /// <inheritdoc/>
        public int GetSensorValueIndexInSample(SensorInfo sensor)
        {
            int disabledWithLowerIdCount =
                sensorInfos.Take(sensor.Id).Count(si => !si.Enabled);
            return sensor.Id - disabledWithLowerIdCount;
        }

        /// <inheritdoc/>
        public IEnumerable<ISample> ReadSamples(int maxCount = int.MaxValue)
        {
            // As per gMOBIlabplusDataFormatv209a.pdf, page 3
            const double ScaleBase = 2 * 5 * 1e-6 / (65536 * 4);

            int sampleCount = Math.Min(maxCount, this.GetAvailableSampleCount());
            int enabledChannelCount =
                sensorInfos.Count(si => si.Enabled);
            var sampleLength = (double)TimeSpan.TicksPerSecond / sampleRate;
            for (int sampleIdx = 0; sampleIdx < sampleCount; ++sampleIdx)
            {
                var sampleData = new BasicSampleData[enabledChannelCount];
                int outputIdx = 0;
                for (int channelId = 0; channelId < sensorInfos.Length; ++channelId)
                {
                    // If the channel was not even recorded, dont read a value.
                    if (!analogChannels[channelId].HasValue)
                    {
                        continue;
                    }

                    var rawValue = dataReader.ReadInt16();

                    // If the channel was recored but was disabled, discard
                    // the read value.
                    if (!sensorInfos[channelId].Enabled)
                    {
                        continue;
                    }

                    // If the channel is enabled it also always recorded.
                    // ReSharper disable once PossibleInvalidOperationException
                    SensorChannel channel = analogChannels[channelId].Value;

                    double scale = channel.Sensitivity * ScaleBase;
                    sampleData[outputIdx] = new BasicSampleData(
                        sensorInfos[channelId], rawValue * scale);
                    ++outputIdx;
                }

                yield return new BasicSample(
                    startTime + TimeSpan.FromTicks((long)(sampleLength * sampleIdx)),
                    sampleData);
            }
        }

        /// <inheritdoc />
        public int GetAvailableSampleCount()
        {
            return (int)(file.Length - file.Position)
                / (sizeof(short) * recordedChannelCount);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            file.Dispose();
        }

        /// <summary>
        ///     Parses a g.tec binary file header in format version 3.0.
        /// </summary>
        private void ParseHeader()
        {
            Func<string, string> checkNoEof = lineStr =>
                {
                    if (lineStr == null)
                    {
                        throw new InvalidDataException("Unexpected EOF.");
                    }

                    return lineStr;
                };

            if (checkNoEof(ReadBinaryLine()) != "gtec")
            {
                throw new InvalidDataException("Bad producer.");
            }

            var product = checkNoEof(ReadBinaryLine());
            if (product != "gMOBIlab+" && product != "g.MOBIlab+")
            {
                throw new InvalidDataException("Bad product.");
            }

            if (checkNoEof(ReadBinaryLine()) != "3.0")
            {
                throw new InvalidDataException("Bad file version.");
            }

            sampleRate = int.Parse(checkNoEof(ReadBinaryLine()));

            #region Parse Channel coding.

            var channelCoding = checkNoEof(ReadBinaryLine());
            if (channelCoding.Length != 8 * 3)
            {
                throw new InvalidDataException("Bad channel coding length.");
            }

            Action<char> checkChanCoding = c =>
                {
                    if (c != '0' && c != '1')
                    {
                        throw new InvalidDataException("Bad character in channel coding.");
                    }
                };

            // Check which analog channels are enabled.
            for (var i = 0; i < 8; ++i)
            {
                checkChanCoding(channelCoding[i]);
                if (channelCoding[i] == '1')
                {
                    // Mark channel as used (actual data is filled in later).
                    this.analogChannels[7 - i] = new SensorChannel();
                    ++recordedChannelCount;
                }
            }

            // Check how many digital channels are enabled.
            for (var i = 8; i < 16; ++i)
            {
                if (channelCoding[i] == '1')
                {
                    ++recordedChannelCount;
                    break;
                }
            }

            #endregion

            checkNoEof(ReadBinaryLine()); // Ignore displayed channels.
            checkNoEof(ReadBinaryLine()); // Ignore displayed time.
            checkNoEof(ReadBinaryLine()); // Ignore hardware version.
            checkNoEof(ReadBinaryLine()); // Ignore serial number.

            #region Parse analog channel information

            for (var i = 0; i < 8; ++i)
            {
                var tokens = checkNoEof(ReadBinaryLine()).Split('/');

                if (!this.analogChannels[i].HasValue)
                {
                    continue;
                }

                this.analogChannels[i] = new SensorChannel
                                        {
                                            Highpass = float.Parse(tokens[0], CultureInfo.InvariantCulture), 
                                            Lowpass = float.Parse(tokens[1], CultureInfo.InvariantCulture), 
                                            Sensitivity =
                                                float.Parse(tokens[2], CultureInfo.InvariantCulture), 
                                            SampleRate =
                                                float.Parse(tokens[3], CultureInfo.InvariantCulture), 
                                            Polarity = (AnalogChannelPolarity)(byte)tokens[4][0]
                                        };
            }

            #endregion

            var str = checkNoEof(ReadBinaryLine());
            if (str != "EOH")
            {
                throw new InvalidDataException("EOH expected.");
            }
        }

        /// <summary>
        /// The read binary line.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ReadBinaryLine()
        {
            var bytes = new List<byte>();

            var readByte = dataReader.ReadByte();

            while (readByte != '\n')
            {
                bytes.Add(readByte);
                readByte = dataReader.ReadByte();
            }

            bytes.RemoveAt(bytes.Count - 1);
            string result = Encoding.ASCII.GetString(bytes.ToArray());
            return result;
        }
    }
}