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
        private TextFieldParser csvFile;

        private int sampleRate;

        private int remainingSamples;

        private readonly SensorInfo[] sensorInfos;

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
            // Note: We assume that the file has no empty lines.
            // -1: Subtract header.
            this.remainingSamples =
                File.ReadLines(filename).TakeWhile(s => !s.StartsWith(";")).Count() - 1;

            csvFile = new TextFieldParser(filename);
            csvFile.Delimiters = new[] {";"};
            var header = csvFile.ReadFields();
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

            var t1 = TimeSpan.Parse(bufferedBegin[0][0]);
            var t2 = TimeSpan.Parse(bufferedBegin[1][0]);
            var dt = t2 - t1;
            if (dt.Ticks < 0)
            {
                throw new InvalidDataException("Negative samplerate.");
            }

            var rawSampleRate = 1.0 / dt.TotalSeconds;
            this.sampleRate = (int)rawSampleRate;

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (rawSampleRate != sampleRate)
            {
                throw new InvalidDataException("Samplerate is not an integer.");
            }
        }

        private static Unit GetUnit(string s)
        {

            if (s.Contains("_SCL"))
            {
                return Unit.MicroSiemens;
            }

            if (s.Contains("_Temp"))
            {
                return Unit.DegreeCelsius;
            }

            if (s.Contains("_Puls"))
            {
                return Unit.Bpm;
            }

            // Resp, BVP, PVA, Mot
            return Unit.Unknown;
        }

        public void Dispose()
        {
            if (csvFile != null)
            {
                csvFile.Dispose();
                csvFile = null;
            }
        }

        public IReadOnlyList<SensorInfo> Sensors
        {
            get { return this.sensorInfos; }
        }

        public bool IsUpdating { get { return false; } }

        public int SampleRate { get { return sampleRate; } }

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

        public int GetAvailableSampleCount()
        {
            return remainingSamples;
        }

        public int GetSensorValueIndexInSample(SensorInfo sensor)
        {
            // TODO Code duplication from SensorDataFileStreamer
            var disabledWithLowerIdCount =
                 sensorInfos.Take(sensor.Id).Count(si => !si.Enabled);
            return sensor.Id - disabledWithLowerIdCount;
        }

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

        private Sample MakeSample(string[] fields) {
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