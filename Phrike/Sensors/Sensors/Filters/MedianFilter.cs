﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationPhrike.Sensors.Filters
{
    class MedianFilter : FilterBase
    {
        public MedianFilter(int radius)
            : base(radius)
        {
            //nothing to do
        }

        protected override double FilterData(int start, int end, int mid, IReadOnlyList<double> unfilteredData)
        {
            double[] sort = unfilteredData.ToArray();
            Array.Sort(sort);

            return sort[(end - start + 1) / 2];
        }
    }
}