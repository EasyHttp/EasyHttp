using System;
using System.IO;
using System.Net;
using System.Runtime.Remoting;
using System.Runtime.Serialization;

namespace EasyHttp.Http.Abstractions
{
    public class HttpWebResponseWrapper : IWebResponse, IHttpWebResponse
    {
        private readonly HttpWebResponse _innerResponse;

        public HttpWebResponseWrapper(HttpWebResponse innerResponse)
        {
            _innerResponse = innerResponse;
        }

        public HttpWebResponse InnerResponse { get { return _innerResponse; } }
        public bool IsMutuallyAuthenticated { get { return _innerResponse.IsMutuallyAuthenticated; } }
        long IWebResponse.ContentLength { get; set; }
        string IWebResponse.ContentType { get; set; }
        public CookieCollection Cookies { get { return _innerResponse.Cookies; } set { _innerResponse.Cookies = value; } }
        public WebHeaderCollection Headers { get { return _innerResponse.Headers; } }
        public bool SupportsHeaders { get { return true; } }
        public long ContentLength { get { return _innerResponse.ContentLength; } }
        public string ContentEncoding { get { return _innerResponse.ContentEncoding; } }
        public string ContentType { get { return _innerResponse.ContentType; } }
        public string CharacterSet { get { return _innerResponse.CharacterSet; } }
        public string Server { get { return _innerResponse.Server; } }
        public DateTime LastModified { get { return _innerResponse.LastModified; } }
        public HttpStatusCode StatusCode { get { return _innerResponse.StatusCode; } }
        public string StatusDescription { get { return _innerResponse.StatusDescription; } }
        public Version ProtocolVersion { get { return _innerResponse.ProtocolVersion; } }
        public Uri ResponseUri { get { return _innerResponse.ResponseUri; } }
        public string Method { get { return _innerResponse.Method; } }
        public bool IsFromCache { get { return _innerResponse.IsFromCache; } }

        public Stream GetResponseStream()
        {
            return _innerResponse.GetResponseStream();
        }

        public void Close()
        {
            _innerResponse.Close();
        }

        public string GetResponseHeader(string headerName)
        {
            return _innerResponse.GetResponseHeader(headerName);
        }

        public object GetLifetimeService()
        {
            return _innerResponse.GetLifetimeService();
        }

        public object InitializeLifetimeService()
        {
            return _innerResponse.InitializeLifetimeService();
        }

        public ObjRef CreateObjRef(Type requestedType)
        {
            return _innerResponse.CreateObjRef(requestedType);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            (_innerResponse as ISerializable).GetObjectData(info, context);
        }

        public void Dispose()
        {
            (_innerResponse as IDisposable).Dispose();
        }
    }
}