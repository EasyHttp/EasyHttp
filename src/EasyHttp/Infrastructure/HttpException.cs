using System;
using System.Net;
using System.Runtime.Serialization;

namespace EasyHttp.Infrastructure
{
    public class HttpException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }
        public string StatusDescription { get; private set; }

        public HttpException(HttpStatusCode statusCode, string statusDescription): base(String.Format("{0} {1}", statusCode, statusDescription))
        {
            StatusCode = statusCode;
            StatusDescription = statusDescription;
        }

        public HttpException()
        {
        }

        public HttpException(string message) : base(message)
        {
        }

        public HttpException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HttpException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

      
    }
}