using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Avalara.AvaTax.Adapter.LogService
{
    [Serializable]
    public class LogTokenArgumentException : Exception
    {
        public LogTokenArgumentException()
        {
        }

        public LogTokenArgumentException(string message)
            : base(message)
        {
        }

        public LogTokenArgumentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected LogTokenArgumentException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
