using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phrike.Sensors
{
    public static class StatisticUtil
    {

        public static double Difference(this IEnumerable<double> arr)
        {
            var min = arr.Min();
            var max = arr.Max();
            return max - min;
        }

        public static  double Sigma (this IEnumerable<double> arr)
        {
            var variance = Variance(arr);
            return Math.Sqrt(variance);
        }

        public static double Variance (this IEnumerable<double> arr)
        {
            var avg = arr.Average();

            double sum = 0;
            int count = 0;

            foreach (double x in arr)
            {
                sum = sum + (x - avg) * (x - avg);
                count++;
            }

            return sum / count;
        }

        public static double Slope (this IEnumerable<double> arr)
        {
            double numerator = 0;
            double denominator = 0;

            int idx = 1;
            double xAvg = arr.Count() / 2;
            double yAvg = arr.Average();

            foreach (var y in arr)
            {
                numerator = numerator + (idx - xAvg) * (y - yAvg);
                denominator = denominator + (idx - xAvg) * (idx - xAvg);
                idx++;
            }
            return numerator / denominator;
        }

        public static double Intercept (this IEnumerable<double> arr)
        {
            return arr.Average() - Slope(arr) * arr.Count() / 2;
        }

        public static double DeterminationCoefficient(this IEnumerable<double> arr)
        {
            double numerator = 0;
            double denominatorX = 0;
            double denominatorY = 0;

            int idx = 1;
            double xAvg = arr.Count() / 2;
            double yAvg = arr.Average();

            foreach (var y in arr)
            {
                numerator = numerator + (idx - xAvg) * (y - yAvg);
                denominatorX = denominatorX + (idx - xAvg) * (idx - xAvg);
                denominatorY = denominatorY + (y - yAvg) * (y - yAvg);
                idx++;
            }

            return numerator / Math.Sqrt(denominatorX * denominatorY);
        }
    }
}
