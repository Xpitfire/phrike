using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationPhrike.Sensors
{
    class FilteredSensorHub : ISensorHub
    {

        public IReadOnlyList<SensorInfo> Sensors
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsUpdating
        {
            get { throw new NotImplementedException(); }
        }

        public void SetSensorEnabled(SensorInfo sensor, bool enabled = true)
        {
            throw new NotImplementedException();
        }

        public int GetAvailableSampleCount()
        {
            throw new NotImplementedException();
        }

        public int GetSensorValueIndexInSample(SensorInfo sensor)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISample> ReadSamples(int maxCount = int.MaxValue)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void SetFilter(SensorInfo sensor, IFilter filter)
        {
            throw new NotImplementedException();
        }
    }
}
