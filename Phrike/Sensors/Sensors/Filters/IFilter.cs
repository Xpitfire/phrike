using System.Collections.Generic;


namespace OperationPhrike.Sensors.Filters
{
    public interface IFilter
    {
        IReadOnlyList<double> Filter(IReadOnlyList<double> unfilteredData);
    }
}
