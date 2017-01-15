using System;
using System.IO;
using System.Net;
using System.Runtime.Remoting;

namespace EasyHttp.Http.Abstractions
{
    public interface IHttpWebResponse
    {
        bool IsMutuallyAuthenticated { get; }
        CookieCollection Cookies { get; set; }
        WebHeaderCollection Headers { get; }
        bool SupportsHeaders { get; }
        long ContentLength { get; }
        String ContentEncoding { get; }
        string ContentType { get; }
        string CharacterSet { get; }
        string Server { get; }
        DateTime LastModified { get; }
        HttpStatusCode StatusCode { get; }
        string StatusDescription { get; }
        Version ProtocolVersion { get; }
        Uri ResponseUri { get; }
        string Method { get; }
        bool IsFromCache { get; }
        Stream GetResponseStream();
        void Close();
        string GetResponseHeader(string headerName);
        object GetLifetimeService();
        object InitializeLifetimeService();
        ObjRef CreateObjRef(Type requestedType);
    }
}