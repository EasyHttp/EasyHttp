using System;
using System.Runtime.Serialization;

namespace EasyHttp.Infrastructure
{
    public class PropertyNotFoundException : Exception
    {
        public string PropertyName { get; private set; }

        public PropertyNotFoundException()
        {
        }

        public PropertyNotFoundException(string propertyName) : base(propertyName)
        {
            PropertyName = propertyName;
        }

        public PropertyNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PropertyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}