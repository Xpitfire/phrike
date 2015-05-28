using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationPhrike.Sensors.Filters
{
    public class GaussFilter : RadiusFilterBase
    {

        private int maskLength;
        private double[] gauss;
        private int p;


        public GaussFilter(int radius)
            : base(radius)
        {
            maskLength = 2 * radius + 1;
            gauss = this.CalculateGaussCurve();
        }

        private double[] CalculateGaussCurve()
        {
            double sigma = Radius / 2.0;
            double sum = 0;
            gauss = new double[maskLength];
            for (int i = 0; i < maskLength; i++)
            {
                gauss[i] =
                    (1 / Math.Sqrt(2 * Math.PI * sigma * sigma)
                    * Math.Exp(-(i - Radius)
                    * (i - Radius)
                    / (2 * sigma * sigma)));
                sum += gauss[i];
            }

            for (int i = 0; i < maskLength; i++)
            {
                gauss[i] = gauss[i] / sum;
            }

            return gauss;

        }

        protected override double FilterData(int start, int end, int mid, IReadOnlyList<double> unfilteredData)
        {

            double sum = 0;
            double gaussMaskSum = 0;

            for (int i = start; i <= end; i++)
            {
                int maskPos = i-start + (Radius - (mid - start));
                sum += unfilteredData[i] * gauss[maskPos];
                gaussMaskSum += gauss[maskPos];
            }

            sum /= gaussMaskSum;

            return sum;

        }
    }
}
