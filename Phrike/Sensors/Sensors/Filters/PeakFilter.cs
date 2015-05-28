using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationPhrike.Sensors.Filters
{
    public class PeakFilter : RadiusFilterBase
    {

        private int lastPeak;

        public PeakFilter(double threshold, int minDistance) : base (1)
        {
            Threshold = threshold;
            MinDistance = minDistance;
        }

        public override IReadOnlyList<double> Filter(IReadOnlyList<double> unfilteredData)
        {
            lastPeak = -MinDistance;
            return base.Filter(unfilteredData);
        }

        protected override double FilterData(
            int start, int end, int mid,
            IReadOnlyList<double> unfilteredData)
        {
            double midVal = unfilteredData[mid];
            double leftVal = start == mid ? double.MinValue : unfilteredData[start];
            double rightVal = end == mid ? double.MinValue : unfilteredData[end];
            if (midVal > Math.Max(leftVal, rightVal) && midVal >= Threshold && mid - lastPeak >= MinDistance)
            {
                lastPeak = mid;
                return 1;
            }
            return 0;
        }

        public double Threshold { get; private set; }

        public int MinDistance { get; private set; }
    }
}
