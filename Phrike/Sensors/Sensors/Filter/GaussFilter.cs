using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationPhrike.Sensors
{
    class GaussFilter : FilterBase
    {

        private int arrayLength;
        private double[] gauss;


        private GaussFilter(int radius)
            : base(radius)
        {
            // nothing to do
        }

        private double[] CalculateGaussCurve()
        {
            int sigma = Radius / 2;
            double sum = 0;
            double[] gauss = new double[Radius];
            for (int i = 0; i < arrayLength; i++)
            {
                gauss[i] = (double)(1 / Math.Sqrt(2 * Math.PI * sigma * sigma) * Math.Exp(-(i - Radius) * (i - Radius) / (2 * sigma * sigma)));
                sum += gauss[i];
            }

            for (int i = 0; i < arrayLength; i++)
            {
                gauss[i] = gauss[i] / sum;
            }

            return gauss;

        }

        public override double[] Filter(double[] unfilteredData)
        {
            arrayLength = unfilteredData.Length;
            gauss = this.CalculateGaussCurve();
            return base.Filter(unfilteredData);
        }

        protected override double FilterData(int start, int end, double[] unfilteredData)
        {

            double sum = 0;

            for (int i = start; i <= end; i++)
            {
                sum += unfilteredData[i] * gauss[i - start];
            }

            return sum;

        }
    }
}
