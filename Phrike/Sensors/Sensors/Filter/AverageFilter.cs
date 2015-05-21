using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationPhrike.Sensors
{
    public class AverageFilter : FilterBase
    {
        public AverageFilter(int radius)
            : base(radius)
        {
            // nothing to do
        }
          

        protected override double FilterData(int start, int end, int mid, double[] unfilteredData)
        {
            double sum = 0;

            for (int i = start; i <= end; i++)
            {
                sum += unfilteredData[i];
            }

            return sum / (end - start + 1);
        }
    }
}
