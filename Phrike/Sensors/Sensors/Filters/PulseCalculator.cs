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

namespace OperationPhrike.Sensors.Filters
{
    /// <summary>
    /// Filter that calculates the pulse rate from prefiltered peak data.
    /// This is not a general filter, it should only be used with the output of
    /// a Heart.
    /// </summary>
    public class PulseCalculator : IFilter
    {
        /// <inheritdoc/>
        public IReadOnlyList<double> Filter(IReadOnlyList<double> peaks)
        {
            const int MinPulse = 30;
            const int MaxPulse = 250;

            int lastPeakPos = -1;
            double sampleDistanceInMs = 1000 / 256.0;
            var pulseRates = new List<double>();
            var result = new double[peaks.Count];

            int lastInsertPos = -1;
            Action<double, int> addPulse = (pulse, pos) =>
                {
                    pulseRates.Add(pulse);
                    for (int i = lastInsertPos + 1; i < pos; ++i)
                    {
                        result[i] = pulse;
                    }

                    lastInsertPos = pos;
                };

            for (int i = 0; i < peaks.Count; i++)
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
                        continue;
                    }

                    if (pulseRates.Count > 0)
                    {
                        double averagePulse = pulseRates.Skip(pulseRates.Count - 3).Average();

                        if (pulse <= averagePulse * 1.2 && pulse < averagePulse * 0.8)
                        {
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

            if (pulseRates.Count >= 0)
            {
                for (int i = lastInsertPos; i < result.Length; ++i)
                {
                    result[i] = pulseRates[pulseRates.Count - 1];
                }    
            }

            return result;
        }
    }
}
