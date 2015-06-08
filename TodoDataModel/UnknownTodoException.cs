using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoDataModel
{
    public class UnknownTodoException : TodoException
    {
        public UnknownTodoException(string message) : base(message)
        {
        }

        public UnknownTodoException(string message, Exception innerEx)
            : base(message, innerEx)
        {
        }
    }
}
