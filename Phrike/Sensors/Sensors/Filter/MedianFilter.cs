using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationPhrike.Sensors
{
    class MedianFilter : FilterBase
    {
        public MedianFilter(int radius)
            : base(radius)
        {
            //nothing to do
        }

        protected override double FilterData(int start, int end, int mid, double[] unfilteredData)
        {
            double[] sort = new double[end - start + 1];
            Array.Copy(unfilteredData, start, sort, 0, end - start + 1);
            Array.Sort(sort);

            return sort[(end - start + 1) / 2];
        }
    }
}
