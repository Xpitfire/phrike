using System.Collections.Generic;


namespace OperationPhrike.Sensors.Filters
{
    interface IFilter
    {
        IReadOnlyList<double> Filter(IReadOnlyList<double> unfilteredData);
    }
}
