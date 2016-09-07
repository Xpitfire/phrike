using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OperationPhrike.GMobiLab;
using OperationPhrike.Sensors;

namespace OperationPhrike.SensorFilters
{
    public enum FilterType
    {
        AverageFilter,
        MedianFilter,
        GaussFilter
    }

    public class Filter
    {
        private double[] unfilteredData;
        private double[] filteredData;
        private int length;
        private int radius;
        private int maskLength;
        private FilterType type;
        private ISample[] sampleArray;

        // gets removed as soon as classes are implemented
        private int sigma;
        private double[] gauss;

        public Filter(ISensorHub s/*, int radius = 3*/) 
        { 
            sampleArray = s.ReadSamples().ToArray();
            length = sampleArray.Length;
            filteredData = new double[length];
            unfilteredData = new double[length];
            radius = 2;
            maskLength = (radius * 2) + 1;

            InitUnfilteredArray(5);

            // radius = this.radius;
            type = FilterType.GaussFilter;
           
            // gets removed as soon as classes are implemented
            sigma = radius / 2;
            gauss = new double[maskLength];
            double sum = 0;
            for (int i = 0; i < maskLength; i++)
            {
                gauss[i] = (double)(1 / Math.Sqrt(2 * Math.PI * sigma * sigma) * Math.Exp(-(i - radius) * (i - radius) / (2 * sigma * sigma)));
                sum += gauss[i];
            }

            for (int i = 0; i < maskLength; i++)
            {
                gauss[i] = gauss[i] / sum;
            }

            sum = 0;
            for (int i = 0; i < maskLength; i++)
            {
                Debug.Write(gauss[i] + " ");
                sum += gauss[i];
            }

            Debug.Write("Sum: " + sum);
        }

        public void ApplyFilter()
        {
            for (int i = 0; i < length; i++)
            {
                FilterData(Math.Max(0, i - 2), Math.Min(i + 2, length - 1), i);
            }
        }

        public void Show()
        {
            for (int i = 0; i < length; i++)
            {
                Debug.Write(filteredData[i] + " ");
            }
        }

        public ISample[] GetFilteredSignal()
        {
            /*
            ISample[] result = new ISample[length];

            for (int i = 0; i < length; i++)
            {
                result[i].Values[5].Value = filteredData[i];
            }
            */
            return sampleArray;
        }

        private void InitUnfilteredArray(int channel)
        {
            for (int i = 0; i < sampleArray.Length; i++)
            {
                unfilteredData[i] = sampleArray[i].Values[channel].Value;
            }
        }

        private void MedianFilter(int start, int end, int pos)
        {
            double[] sort = new double[end - start + 1];
            Array.Copy(unfilteredData, start, sort, 0, end - start + 1);
            Array.Sort(sort);

            filteredData[pos] = sort[(end - start + 1) / 2];
        }

        private void AverageFilter(int start, int end, int pos)
        {
            double sum = 0;

            for (int i = start; i <= end; i++)
            {
                sum += unfilteredData[i];
            }
           
            filteredData[pos] = sum / (end - start + 1);
        }

        private void GaussFilter(int start, int end, int pos)
        {
            double sum = 0;

            for (int i = start; i <= end; i++)
            {
                sum += unfilteredData[i] * gauss[i - start];
            }

            filteredData[pos] = sum;
        }

        private void UnknownFilter(int start, int end, int pos)
        {
            double sum = 0;

            for (int i = start; i <= end; i++)
            {
                sum += unfilteredData[i];
            }

            filteredData[pos] = sum / (end - start + 1);
        }

        private void FilterData(int start, int end, int pos)
        {
            if (type == FilterType.AverageFilter)
            {
                AverageFilter(start, end, pos);
            }
            else if (type == FilterType.MedianFilter)
            {
                MedianFilter(start, end, pos);
            }
            else if (type == FilterType.GaussFilter)
            {
                GaussFilter(start, end, pos);
            }
        }
    }
}
