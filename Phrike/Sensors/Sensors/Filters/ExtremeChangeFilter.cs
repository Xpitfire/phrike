using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationPhrike.Sensors.Filters
{
    public class ExtremeChangeFilter : FilterBase
    {

        private double lastValidVal;

        public ExtremeChangeFilter(int lVV, double maxYDistance)
            : base(1)
        {
            lastValidVal = lVV;
            MaxYDistance = maxYDistance;
        }

        protected override double FilterData(int start, int end, int mid, IReadOnlyList<double> unfilteredData)
        {

            if (Math.Abs(unfilteredData[mid] - lastValidVal) > MaxYDistance)
            {
                return lastValidVal;
            }
            lastValidVal = unfilteredData[mid];
            return unfilteredData[mid];

        }

        public double MaxYDistance { get; private set; }

    }
}
