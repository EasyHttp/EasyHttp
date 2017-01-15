using System;
using System.IO;
using System.Net;
using System.Runtime.Remoting;
using System.Runtime.Serialization;

namespace EasyHttp.Http.Abstractions
{
    public class WebResponseWrapper : IWebResponse
    {
        private readonly WebResponse _innerRespose;

        public WebResponseWrapper(WebResponse innerRespose)
        {
            _innerRespose = innerRespose;
        }

        public void Close()
        {
            _innerRespose.Close();
        }

        public bool IsFromCache { get { return _innerRespose.IsFromCache; } }
        public bool IsMutuallyAuthenticated { get { return _innerRespose.IsMutuallyAuthenticated; } }
        public long ContentLength { get { return _innerRespose.ContentLength; } set { _innerRespose.ContentLength = value; } }
        public string ContentType { get { return _innerRespose.ContentType; } set { _innerRespose.ContentType = value; } }
        public Uri ResponseUri { get { return _innerRespose.ResponseUri; } }
        public WebHeaderCollection Headers { get { return _innerRespose.Headers; } }
        public bool SupportsHeaders { get { return false; } }

        public Stream GetResponseStream()
        {
            return _innerRespose.GetResponseStream();
        }

        public object GetLifetimeService()
        {
            return _innerRespose.GetLifetimeService();
        }

        public object InitializeLifetimeService()
        {
            return _innerRespose.InitializeLifetimeService();
        }

        public ObjRef CreateObjRef(Type requestedType)
        {
            return _innerRespose.CreateObjRef(requestedType);
        }

        public void Dispose()
        {
            (_innerRespose as IDisposable).Dispose();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            (_innerRespose as ISerializable).GetObjectData(info, context);
        }
    }
}