// <summary>Implementation of the PulseCalculator filter.</summary>
// -----------------------------------------------------------------------
// Copyright (c) 2015 University of Applied Sciences Upper-Austria
// Project OperationPhrike
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR 
// ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION 
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;

namespace Phrike.Sensors.Filters
{
    /// <summary>
    /// Filter that calculates the pulse rate from prefiltered peak data.
    /// This is not a general filter, it should only be used with the output of
    /// a Heart.
    /// </summary>
    /// <remarks>
    /// The pulse is calculated at peaks. The samples before are then filled
    /// with this pulse rate. The samples after the last known pulse are filled
    /// with the last pulse rate. If no pulse at all can be detected, the result
    /// is filled with zeros.
    /// </remarks>
    public class PulseCalculator : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PulseCalculator"/> class. 
        /// Constructor of PulseCalculator.
        /// </summary>
        /// <param name="filter">
        /// Filter that is applied during the pulse calculation.
        /// </param>
        /// <param name="sampleRate">
        /// The sample Rate.
        /// </param>
        public PulseCalculator(IFilter filter = null, int sampleRate = 256)
        {
            PulseFilter = filter;
            SampleRate = sampleRate;
        }

        /// <summary>
        /// Gets the filter that is applied during the pulse calculation.
        /// </summary>
        public IFilter PulseFilter { get; }

        /// <summary>
        /// Gets or sets the sample rate.
        /// </summary>
        public int SampleRate { get; set; }

        /// <summary>
        /// Create a filter chain to calculate the pulse from completely unfiltered sensor data.
        /// </summary>
        /// <returns>The pulse rate at each sample.</returns>
        public static FilterChain MakePulseFilterChain()
        {
            return new FilterChain(
                new GaussFilter(4),
                new EdgeDetectionFilter(2),
                new HeartPeakFilter(
                    new FilterChain( // maxPeakFilter
                        new PeakFilter(15),
                        new BinaryThresholdFilter(0.5)),
                    new FilterChain( // minPeakFilter
                        new PeakFilter(15, false),
                        new BinaryThresholdFilter(0.5, false)),
                    11), // maxPeakDistance
                new PulseCalculator());
        }

        /// <inheritdoc/>
        public IReadOnlyList<double> Filter(IReadOnlyList<double> peaks)
        {
            const int MinPulse = 30;
            const int MaxPulse = 250;

            int lastPeakPos = -1;
            double sampleDistanceInMs = 1000.0 / SampleRate;
            var pulseRates = new List<double>();
            var pulseLengths = new List<int>();
           
            int lastInsertPos = -1;
            Action<double, int> addPulse = (pulse, pos) =>
                {
                    pulseRates.Add(pulse);
                    pulseLengths.Add(pos - lastInsertPos);
                    lastInsertPos = pos;
                };

            for (int i = 0; i < peaks.Count; ++i)
            {
                if (peaks[i] <= 0)
                {
                    continue;
                }

                if (lastPeakPos >= 0)
                {
                    int distance = i - lastPeakPos;
                    double timeMs = distance * sampleDistanceInMs;
                    double pulse = (60 * 1000) / timeMs;
                    if (pulse < MinPulse || pulse > MaxPulse)
                    {
                        lastPeakPos = i;
                        continue;
                    }

                    if (pulseRates.Count > 0)
                    {
                        double averagePulse = pulseRates.Skip(pulseRates.Count - 3).Average();

                        if (pulse < averagePulse * 0.8)
                        {
                            // pulse has to be corrected
                            double oneMissingPulse = pulse * 2;
                            double twoMissingPulse = pulse * 3;
                            double oneMissingToAvg = Math.Abs(averagePulse - oneMissingPulse);
                            double twoMissingToAvg = Math.Abs(averagePulse - twoMissingPulse);
                            double correctedPulse = oneMissingPulse;
                            if (twoMissingToAvg < oneMissingToAvg)
                            {
                                correctedPulse = twoMissingPulse;
                            }

                            addPulse(correctedPulse, i);
                        }
                        else if (pulse >= averagePulse * 1.2)
                        {
                            // NOP
                        }
                        else
                        {
                            addPulse(pulse, i);
                        }
                    }
                    else
                    {
                        addPulse(pulse, i);
                    }
                }

                lastPeakPos = i;
            }

            // 1. Filter pulseRates
            IReadOnlyList<double> filteredPulse = PulseFilter != null ?
                PulseFilter.Filter(pulseRates) : pulseRates;

            // 2. Create result from filteredPulse & pulseLengths
            var result = new double[peaks.Count];

            int outIdx = 0;
            for (int i = 0; i < pulseLengths.Count; i++)
            {
                for (int j = 0; j < pulseLengths[i]; j++)
                {
                    result[outIdx] = filteredPulse[i];
                    ++outIdx;
                }
            }
            
            if (pulseRates.Count > 0)
            {
                for (int i = lastInsertPos + 1; i < result.Length; ++i)
                {
                    result[i] = pulseRates[pulseRates.Count - 1];
                }    
            }
            
            return result;
        }
    }
}
