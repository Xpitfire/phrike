using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationPhrike.Sensors.Filters
{
    using System.Diagnostics.CodeAnalysis;

    public class ValueDistanceFilter : IFilter
    {
        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        virtual public IReadOnlyList<double> Filter(IReadOnlyList<double> unfilteredData)
        {
            int cnt = 0;
            bool peakFound = false;
            var result = new List<double>();
            for (int i = 0; i < unfilteredData.Count; i++)
            {
                if (unfilteredData[i] == 0.0)
                {
                    cnt++;
                }
                else if (unfilteredData[i] == 1.0)
                {
                    if (peakFound)
                    {
                        result.Add(cnt + 1);
                    }
                    peakFound = true;
                    cnt = 0;
                }
            }
            return result;
        }

    }
}
