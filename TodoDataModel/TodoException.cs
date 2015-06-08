using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoDataModel
{
    /// <summary>
    /// Base class for all exceptions
    /// </summary>
    public abstract class TodoException : Exception
    {
        protected TodoException(string message) : base(message) { }

        protected TodoException(string message, Exception innerEx) : base(message, innerEx) { }
    }
}
