// <summary>Implementation file for DataSeries.</summary>
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

namespace Phrike.Sensors
{
    /// <summary>
    /// A series of data, typically a possibly filtered sensor.
    /// </summary>
    public class DataSeries
    {
        private Statistics statistics;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSeries"/> class.
        /// </summary>
        /// <param name="data"><see cref="Data"/></param>
        /// <param name="sampleRate"><see cref="SampleRate"/></param>
        /// <param name="sourceName"><see cref="SourceName"/></param>
        /// <param name="name"><see cref="Name"/></param>
        /// <param name="unit"><see cref="Unit"/></param>
        public DataSeries(double[] data, int sampleRate, string sourceName, string name, Unit unit)
        {
            Data = data;
            SampleRate = sampleRate;
            SourceName = sourceName;
            Name = name;
            Unit = unit;
        }

        /// <summary>
        /// Gets the actual data, with a sample rate of <see cref="SampleRate"/>.
        /// </summary>
        public double[] Data { get; }

        /// <summary>
        /// Gets the sample rate in samples per second (Hz).
        /// </summary>
        public int SampleRate { get; }

        /// <summary>
        ///Gets the name of the data series' source, usually the sensor hub name.
        /// </summary>
        public string SourceName { get; }

        /// <summary>
        /// Gets the name of the data series. Unique when combined with <see cref="SourceName"/>.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the unit of the data or <see cref="Unit.Unknown"/>.
        /// </summary>
        public Unit Unit { get; }

        /// <summary>
        /// Gets the cached statistics for this <see cref="DataSeries"/>.
        /// </summary>
        public Statistics Statistics => statistics ?? (statistics = Statistics.FromDataSeries(this));
    }
}