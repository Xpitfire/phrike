using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationPhrike.GMobiLab
{
    /// <summary>
    /// A call to the GMobiLab Xfer Helpers C API failed.
    /// </summary>
    public class GMobiLabXferException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GMobiLabXferException"/> class.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        /// <param name="inner">The original exception (if any).</param>
        public GMobiLabXferException(string message, Exception inner = null)
            : base(message, inner)
        {
        }
    }
}
