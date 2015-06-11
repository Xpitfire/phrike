using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phrike.Sensors.Filters
{
    public class PeakFilter : RadiusFilterBase
    {
        public PeakFilter(int radius, bool detectMaxima = true) : base (radius)
        {
            DetectMaxima = detectMaxima;
        }

        protected override double FilterData(
            int start, int end, int mid,
            IReadOnlyList<double> unfilteredData)
        {
            double midVal = unfilteredData[mid];
            bool isExtreme = true;
            for (int i = start; i <= end && isExtreme; i++)
            {
               double actVal = unfilteredData[i];
                if ((actVal > midVal && DetectMaxima)
                    || (actVal < midVal && !DetectMaxima))
                {
                    isExtreme = false;
                }

            }
            return isExtreme ? midVal : 0;
        }

        public bool DetectMaxima { get; private set; }
    }
}
