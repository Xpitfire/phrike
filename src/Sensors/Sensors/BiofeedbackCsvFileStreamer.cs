// <summary>Implements BiofeedbackCsvFileStreamer.</summary>
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
using System.IO;
using System.Linq;

using Microsoft.VisualBasic.FileIO;

namespace Phrike.Sensors
{
    /// <summary>
    /// Reads a CSV file corresponding to a Biofeedback2000 XLS file.
    /// </summary>
    public class BiofeedbackCsvFileStreamer : ISensorHub
    {
        /// <summary>
        /// Holds information for each column in the CSV file that represents a sensor.
        /// </summary>
        private readonly SensorInfo[] sensorInfos;

        /// <summary>
        /// Holds the file handle and parses the CSV file.
        /// </summary>
        private TextFieldParser csvFile;

        /// <summary>
        /// The sample rate of the data in the CSV file.
        /// </summary>
        private int sampleRate;

        /// <summary>
        /// Counts how many samples remain in the CSV file.
        /// </summary>
        private int remainingSamples;

        /// <summary>
        /// Necessary to determine the sample rate in advance. Buffers up to
        /// the first two non-header lines.
        /// </summary>
        private List<string[]> bufferedBegin;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiofeedbackCsvFileStreamer"/>
        /// class.
        /// </summary>
        /// <param name="filename">
        /// Name of the file to read from.
        /// </param>
        public BiofeedbackCsvFileStreamer(string filename)
        {
            Name = filename;

            // Note: We assume that the file has no empty lines.
            // -1: Subtract header.
            this.remainingSamples =
                File.ReadLines(filename).TakeWhile(s => !s.StartsWith(";")).Count() - 1;

            csvFile = new TextFieldParser(filename) { Delimiters = new[] { ";" } };
            string[] header = csvFile.ReadFields();
            if (header == null)
            {
                throw new InvalidDataException("Empty file.");
            }

            if (header[header.Length - 1].Trim() == "Bemerkung")
            {
                header[header.Length - 1] = null;
            }

            this.sensorInfos = header
                .Skip(1) // Time column is no sensor.
                .Where(s => s != null)
                .Select((s, idx) => new SensorInfo(s, GetUnit(s), true, idx))
                .ToArray();
            bufferedBegin = new List<string[]>
            {
                csvFile.ReadFields(), csvFile.ReadFields()
            };

            TimeSpan t1 = TimeSpan.Parse(bufferedBegin[0][0]);
            TimeSpan t2 = TimeSpan.Parse(bufferedBegin[1][0]);
            TimeSpan dt = t2 - t1;
            if (dt.Ticks < 0)
            {
                throw new InvalidDataException("Negative samplerate.");
            }

            double rawSampleRate = 1.0 / dt.TotalSeconds;
            this.sampleRate = (int)rawSampleRate;

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (rawSampleRate != sampleRate)
            {
                throw new InvalidDataException("Samplerate is not an integer.");
            }
        }

        /// <inheritdoc />
        public bool IsUpdating { get { return false; } }

        /// <inheritdoc />
        public int SampleRate { get { return sampleRate; } }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public IReadOnlyList<SensorInfo> Sensors
        {
            get { return this.sensorInfos; }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (csvFile != null)
            {
                csvFile.Dispose();
                csvFile = null;
            }
        }

        /// <inheritdoc />
        public void SetSensorEnabled(SensorInfo sensor, bool enabled = true)
        {
            // TODO Code duplication from SensorDataFileStreamer
            if (sensor.Id < 0 || sensor.Id >= this.sensorInfos.Length)
            {
                throw new ArgumentException("Sensor ID out of range.", nameof(sensor));
            }

            var oldInfo = this.sensorInfos[sensor.Id];
            this.sensorInfos[sensor.Id] = oldInfo.ToEnabled(enabled);
        }

        /// <inheritdoc />
        public int GetAvailableSampleCount()
        {
            return remainingSamples;
        }

        /// <inheritdoc />
        public int GetSensorValueIndexInSample(SensorInfo sensor)
        {
            // TODO Code duplication from SensorDataFileStreamer
            var disabledWithLowerIdCount =
                 sensorInfos.Take(sensor.Id).Count(si => !si.Enabled);
            return sensor.Id - disabledWithLowerIdCount;
        }

        /// <inheritdoc />
        public IEnumerable<Sample> ReadSamples(int maxCount = Int32.MaxValue)
        {
            maxCount = Math.Min(maxCount, remainingSamples);
            while (maxCount > 0 && bufferedBegin != null)
            {
                --maxCount;
                yield return MakeSample(bufferedBegin.First());
                bufferedBegin.RemoveAt(0);
                if (bufferedBegin.Count <= 0)
                {
                    bufferedBegin = null;
                }
            }

            while (maxCount > 0)
            {
                --maxCount;

                var line = csvFile.ReadFields();
                if (line == null)
                {
                    yield break;
                }

                yield return MakeSample(line);
            }
        }

        /// <summary>
        /// Gets the unit of the column with the given column header.
        /// </summary>
        /// <param name="columnName">
        /// Name of the column for which to retrieve the unit.
        /// </param>
        /// <returns>
        /// The unit of the data in the column with the given name.
        /// </returns>
        private static Unit GetUnit(string columnName)
        {
            if (columnName.Contains("_SCL"))
            {
                return Unit.MicroSiemens;
            }

            if (columnName.Contains("_Temp"))
            {
                return Unit.DegreeCelsius;
            }

            if (columnName.Contains("_Puls"))
            {
                return Unit.Bpm;
            }

            // Resp, BVP, PVA, Mot
            return Unit.Unknown;
        }

        /// <summary>
        /// Creates a sample from a parsed CSV line.
        /// </summary>
        /// <param name="fields">The fields of a CSV line.</param>
        /// <returns>A sample containing the data from <paramref name="fields"/> as a <see cref="Sample"/>.</returns>
        private Sample MakeSample(string[] fields)
        {
            --remainingSamples;
            return new Sample(
                fields
                    .Skip(1) // Skip timestamp
                    .Take(sensorInfos.Length)
                    .Where((s, i) => sensorInfos[i].Enabled)
                    .Select(double.Parse)
                .ToArray());
        }
    }
}