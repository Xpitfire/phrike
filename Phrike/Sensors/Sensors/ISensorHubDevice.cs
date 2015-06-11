// <summary>Specifies interfaces for a sensor hub device.</summary>
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
    /// Represents a sensor hub that can actually record new data.
    /// </summary>
    public interface ISensorHubDevice : ISensorHub
    {
        /// <summary>
        /// Start recording sensor data.
        /// </summary>
        /// <remarks>
        /// This method may block, but it might also take some time before new
        /// samples actually become available.
        /// </remarks>
        void StartRecording();

        /// <summary>
        /// Stops recording sensor data.
        /// </summary>
        void StopRecording();
    }
}
