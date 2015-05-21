using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationPhrike.Sensors
{
    class AverageFilter : FilterBase
    {
        public AverageFilter(int radius)
            : base(radius)
        {
            
        }
          

        protected override double FilterData(int start, int end, double[] unfilteredData)
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
