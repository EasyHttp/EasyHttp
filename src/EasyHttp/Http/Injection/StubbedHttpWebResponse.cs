using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting;
using EasyHttp.Http.Abstractions;

namespace EasyHttp.Http.Injection
{
    public class StubbedHttpWebResponse : IHttpWebResponse
    {
        private readonly string _responseBody;

        public bool IsMutuallyAuthenticated { get; private set; }
        public CookieCollection Cookies { get; set; }
        public WebHeaderCollection Headers { get; private set; }
        public bool SupportsHeaders { get; private set; }
        public long ContentLength { get { return _responseBody.Length; } }
        public string ContentEncoding { get{ return Headers.Get("Content-Encoding"); } }
        public string ContentType { get { return Headers.Get("Content-Type"); } }
        public string CharacterSet { get; private set; }
        public string Server { get; private set; }
        public DateTime LastModified { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
        public string StatusDescription { get; private set; }
        public Version ProtocolVersion { get; private set; }
        public Uri ResponseUri { get; private set; }
        public string Method { get; private set; }
        public bool IsFromCache { get; private set; }

        public StubbedHttpWebResponse(HttpStatusCode statusCode, Dictionary<HttpResponseHeader, string> responseHeaders, string responseBody)
        {
            StatusCode = statusCode;

            Headers = new WebHeaderCollection();
            if (responseHeaders != null)
            {
                foreach (var header in responseHeaders)
                {
                    Headers.Add(header.Key, header.Value);
                }
            }

            _responseBody = responseBody;
        }

        public Stream GetResponseStream()
        {
            return _responseBody.ToStream();
        }

        public void Close()
        {
            // TODO
        }

        public string GetResponseHeader(string headerName)
        {
            return Headers.AllKeys.Contains(headerName)
                ? string.Join(",", Headers.GetValues(headerName))
                : string.Empty;
        }

        public object GetLifetimeService()
        {
            throw new NotImplementedException("TODO");
        }

        public object InitializeLifetimeService()
        {
            throw new NotImplementedException("TODO");
        }

        public ObjRef CreateObjRef(Type requestedType)
        {
            throw new NotImplementedException("TODO");
        }
    }
}