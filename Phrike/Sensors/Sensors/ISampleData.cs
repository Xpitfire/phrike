// <summary>Specifies interfaces for sensor data sources.</summary>
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
namespace OperationPhrike.Sensors
{
    /// <summary>
    ///     A data value of a single sensor in a single sample.
    /// </summary>
    public interface ISampleData
    {
        /// <summary>
        ///     Gets the Sensor that recorded the data.
        /// </summary>
        SensorInfo Source { get; }

        /// <summary>
        ///     Gets the sensor value.
        /// </summary>
        double Value { get; }
    }
}