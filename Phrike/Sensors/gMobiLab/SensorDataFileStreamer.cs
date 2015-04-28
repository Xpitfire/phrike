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
using System.Text;

namespace OperationPhrike.GMobiLab
{
    /// <summary>
    ///     Reads a data file as written to the SDCard by the sensor device.
    /// </summary>
    public sealed class SensorDataFileStreamer : ISensorDataSource
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
        /// Initializes a new instance of the <see cref="SensorDataFileStreamer"/> class. 
        /// Initializes a new instance of the
        ///     <see cref="SensorDataFileStreamer"/> class.
        /// </summary>
        /// <param name="filename">
        /// Path to an existing sensor binary file.
        /// </param>
        public SensorDataFileStreamer(string filename)
        {
            file = new FileStream(filename, FileMode.Open);
            dataReader = new BinaryReader(file);

            ParseHeader();
        }

        /// <summary>
        ///     Gets a value indicating whether this is a dynamic data source
        ///     (always false).
        /// </summary>
        public bool IsDynamic
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc />
        public SensorChannel?[] AnalogChannels { get; private set; }

        /// <inheritdoc />
        public DigitalChannelDirection[] DigitalChannels { get; private set; }

        /// <inheritdoc />
        public int GetAvailableDataCount()
        {
            return (int)(file.Length - file.Position) / sizeof(short);
        }

        /// <inheritdoc />
        public short[] GetData(int maxCount)
        {
            var result = new short[Math.Min(maxCount, this.GetAvailableDataCount())];
            for (var i = 0; i < result.Length; ++i)
            {
                result[i] = dataReader.ReadInt16();
            }

            return result;
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

            checkNoEof(ReadBinaryLine()); // Ignore sampling frequency.

            #region Parse Channel coding.

            AnalogChannels = new SensorChannel?[8];
            DigitalChannels = new DigitalChannelDirection[8];
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

            for (var i = 0; i < 8; ++i)
            {
                checkChanCoding(channelCoding[i]);
                if (channelCoding[i] == '1')
                {
                    // Mark channel as used (actual data is filled in later).
                    AnalogChannels[7 - i] = new SensorChannel();
                }
            }

            for (var i = 8; i < 16; ++i)
            {
                var chanIdx = 7 - (i - 8);
                checkChanCoding(channelCoding[i]);
                if (channelCoding[i] == '1')
                {
                    DigitalChannels[chanIdx] = channelCoding[i + 8] == '1'
                                                   ? DigitalChannelDirection.In
                                                   : DigitalChannelDirection.Out;
                }
                else
                {
                    DigitalChannels[chanIdx] = DigitalChannelDirection.Disabled;
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

                if (!AnalogChannels[i].HasValue)
                {
                    continue;
                }

                AnalogChannels[i] = new SensorChannel
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
            string result;
            var bytes = new List<byte>();

            var readByte = dataReader.ReadByte();

            while (readByte != '\n')
            {
                bytes.Add(readByte);
                readByte = dataReader.ReadByte();
            }

            bytes.RemoveAt(bytes.Count - 1);
            result = Encoding.ASCII.GetString(bytes.ToArray());
            return result;
        }
    }
}