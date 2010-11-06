using System;
using System.Net;
using System.Runtime.Serialization;

namespace EasyHttp
{
    public class PropertyNotFoundException : Exception
    {
        public PropertyNotFoundException()
        {
        }

        public PropertyNotFoundException(string message) : base(message)
        {

        }

        public PropertyNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PropertyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}