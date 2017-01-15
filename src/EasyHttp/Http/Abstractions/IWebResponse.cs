using System;
using System.IO;
using System.Net;
using System.Runtime.Remoting;
using System.Runtime.Serialization;

namespace EasyHttp.Http.Abstractions
{
    public interface IWebResponse : ISerializable, IDisposable
    {
        void Close();
        bool IsFromCache { get; }
        bool IsMutuallyAuthenticated { get; }
        long ContentLength { get; set; }
        string ContentType { get; set; }
        Uri ResponseUri { get; }
        WebHeaderCollection Headers { get; }
        bool SupportsHeaders { get; }
        Stream GetResponseStream();
        object GetLifetimeService();
        object InitializeLifetimeService();
        ObjRef CreateObjRef(Type requestedType);
    }
}