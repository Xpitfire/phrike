// <summary>See <see cref="OperationPhrike.GMobiLab.GMobiLabException"/>.</summary>
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

namespace OperationPhrike.GMobiLab
{
    /// <summary>
    /// A call to the GMobiLab C API failed.
    /// </summary>
    public class GMobiLabException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GMobiLabException"/>
        /// class with the given error code.
        /// </summary>
        /// <param name="errorCode">
        /// A error code obtained from a call to
        /// <see cref="GMobiLabApi.GetLastError"/>.
        /// </param>
        /// <param name="inner">The original exception.</param>
        public GMobiLabException(GMobiLabErrorCode errorCode, Exception inner = null)
            : base(null, inner)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GMobiLabException"/>
        /// class, calling <see cref="GMobiLabApi.GetLastError"/> to obtain
        /// the error code.
        /// </summary>
        /// <param name="inner">The original exception (if any).</param>
        public GMobiLabException(Exception inner = null)
            : base(null, inner)
        {
            GMobiLabErrorCode code;
            if (!GMobiLabApi.GetLastError(out code))
            {
                // Should never happen.
                throw new InvalidOperationException(
                    "Failed obtaining last error code.");
            }

            this.ErrorCode = code;
        }

        /// <summary>
        /// Gets the GMobiLab error code.
        /// </summary>
        public GMobiLabErrorCode ErrorCode { get; private set; }

        /// <summary>
        /// Get the GMobiLab error message associated with <see cref="ErrorCode"/>.
        /// </summary>
        public override string Message
        {
            get
            {
                GMobiLabApi.ErrorString errStr;
                if (GMobiLabApi.TranslateErrorCode(out errStr, this.ErrorCode))
                {
                    return errStr.Error;
                }

                return
                    "GMobiLab call failed with unknown error "
                    + this.ErrorCode + ".";
            }
        }
    }
}
