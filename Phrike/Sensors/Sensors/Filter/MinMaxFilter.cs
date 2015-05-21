﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationPhrike.Sensors.Filter
{
    class MinMaxFilter : FilterBase
    {
        public MinMaxFilter(int radius)
            : base(radius)
        {
            // nothing to do
        }


        protected override double FilterData(int start, int end, int mid, double[] unfilteredData)
        {
            int midFactor = end - start;

            double sum = 0;
            
            for (int i = start; i <= end; i++)
            {
                if (i == mid)
                {
                    sum += unfilteredData[i] * midFactor;
                }
                else
                {
                    sum -= unfilteredData[i];
                }
            }

            return sum;

        }
    }
}
