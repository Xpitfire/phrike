// <summary></summary>
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

using Phrike.Sensors;

namespace Sensors.Test
{
    public class FixedSampleSensorHub : ISensorHub
    {
        private int index;

        public void Dispose()
        {
            Samples = null;
            WriteableSensors = null;
        }

        public SensorInfo[] WriteableSensors { get; set; }

        public IList<Sample> Samples { get; set; }

        public IReadOnlyList<SensorInfo> Sensors => WriteableSensors;

        public bool IsUpdating { get; } = false;

        public int SampleRate { get; set; } 

        public string Name { get; set; }

        public void SetSensorEnabled(SensorInfo sensor, bool enabled = true)
        {
            WriteableSensors[index] = Sensors[index].ToEnabled(enabled);
        }

        public int GetAvailableSampleCount()
        {
            return Samples.Count - index;
        }

        public int GetSensorValueIndexInSample(SensorInfo sensor)
        {
            // TODO Code duplication from SensorDataFileStreamer and BiofeedbackCsvFileStreamer.
            int disabledWithLowerIdCount =
                Sensors.Take(sensor.Id).Count(si => !si.Enabled);
            return sensor.Id - disabledWithLowerIdCount;
        }

        public IEnumerable<Sample> ReadSamples(int maxCount = Int32.MaxValue)
        {
            maxCount = Math.Min(maxCount, GetAvailableSampleCount());
            int i;
            for (i = 0; i < maxCount; ++i)
            {
                yield return Samples[index + i];
            }
            index = index + i;
        }
    }
}