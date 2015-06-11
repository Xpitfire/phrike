// <summary>Implements GMobiLabXferException.</summary>
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

namespace Phrike.GMobiLab
{
    /// <summary>
    ///     A call to the GMobiLab Xfer Helpers C API failed.
    /// </summary>
    public class GMobiLabXferException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GMobiLabXferException"/> class. 
        /// Initializes a new instance of the
        ///     <see cref="GMobiLabXferException"/> class.
        /// </summary>
        /// <param name="message">
        /// A message describing the error.
        /// </param>
        /// <param name="inner">
        /// The original exception (if any).
        /// </param>
        public GMobiLabXferException(string message, Exception inner = null)
            : base(message, inner)
        {
        }
    }
}