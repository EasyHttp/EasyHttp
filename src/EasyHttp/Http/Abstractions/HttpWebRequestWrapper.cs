using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Runtime.Remoting;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace EasyHttp.Http.Abstractions
{
    public class HttpWebRequestWrapper : IHttpWebRequest
    {
        private readonly HttpWebRequest _innerRequest;

        public HttpWebRequestWrapper(HttpWebRequest innerRequest)
        {
            _innerRequest = innerRequest;
        }

        #region Properties

        public HttpWebRequest InnerRequest { get { return _innerRequest; } }
        public bool AllowAutoRedirect { get { return _innerRequest.AllowAutoRedirect; } set { _innerRequest.AllowAutoRedirect = value; } }
        public bool AllowWriteStreamBuffering { get { return _innerRequest.AllowWriteStreamBuffering; } set { _innerRequest.AllowWriteStreamBuffering = value; } }
        public bool HaveResponse { get { return _innerRequest.HaveResponse; } }
        public bool KeepAlive { get { return _innerRequest.KeepAlive; } set { _innerRequest.KeepAlive = value; } }
        public bool Pipelined { get { return _innerRequest.Pipelined; } set { _innerRequest.Pipelined = value; } }
        public bool PreAuthenticate { get { return _innerRequest.PreAuthenticate; } set { _innerRequest.PreAuthenticate = value; } }
        public bool UnsafeAuthenticatedConnectionSharing { get { return _innerRequest.UnsafeAuthenticatedConnectionSharing; } set { _innerRequest.UnsafeAuthenticatedConnectionSharing = value; } }
        public bool SendChunked { get { return _innerRequest.SendChunked; } set { _innerRequest.SendChunked = value; } }
        public DecompressionMethods AutomaticDecompression { get { return _innerRequest.AutomaticDecompression; } set { _innerRequest.AutomaticDecompression = value; } }
        public int MaximumResponseHeadersLength { get { return _innerRequest.MaximumResponseHeadersLength; } set { _innerRequest.MaximumResponseHeadersLength = value; } }
        public X509CertificateCollection ClientCertificates { get { return _innerRequest.ClientCertificates; } set { _innerRequest.ClientCertificates = value; } }
        public CookieContainer CookieContainer { get { return _innerRequest.CookieContainer; } set { _innerRequest.CookieContainer = value; } }
        public bool SupportsCookieContainer { get { return true; } }
        public Uri RequestUri { get { return _innerRequest.RequestUri; } }
        public long ContentLength { get { return _innerRequest.ContentLength; } set { _innerRequest.ContentLength = value; } }
        public int Timeout { get { return _innerRequest.Timeout; } set { _innerRequest.Timeout = value; } }
        public int ReadWriteTimeout { get { return _innerRequest.ReadWriteTimeout; } set { _innerRequest.ReadWriteTimeout = value; } }
        public Uri Address { get { return _innerRequest.Address; } }
        public HttpContinueDelegate ContinueDelegate { get { return _innerRequest.ContinueDelegate; } set { _innerRequest.ContinueDelegate = value; } }
        public ServicePoint ServicePoint { get { return _innerRequest.ServicePoint; } }
        public string Host { get { return _innerRequest.Host; } set { _innerRequest.Host = value; } }
        public int MaximumAutomaticRedirections { get { return _innerRequest.MaximumAutomaticRedirections; } set { _innerRequest.MaximumAutomaticRedirections = value; } }
        public string Method { get { return _innerRequest.Method; } set { _innerRequest.Method = value; } }
        public ICredentials Credentials { get { return _innerRequest.Credentials; } set { _innerRequest.Credentials = value; } }
        public bool UseDefaultCredentials { get { return _innerRequest.UseDefaultCredentials; } set { _innerRequest.UseDefaultCredentials = value; } }
        public string ConnectionGroupName { get { return _innerRequest.ConnectionGroupName; } set { _innerRequest.ConnectionGroupName = value; } }
        public WebHeaderCollection Headers { get { return _innerRequest.Headers; } set { _innerRequest.Headers = value; } }
        public IWebProxy Proxy { get { return _innerRequest.Proxy; } set { _innerRequest.Proxy = value; } }
        public Version ProtocolVersion { get { return _innerRequest.ProtocolVersion; } set { _innerRequest.ProtocolVersion = value; } }
        public string ContentType { get { return _innerRequest.ContentType; } set { _innerRequest.ContentType = value; } }
        public string MediaType { get { return _innerRequest.MediaType; } set { _innerRequest.MediaType = value; } }
        public string TransferEncoding { get { return _innerRequest.TransferEncoding; } set { _innerRequest.TransferEncoding = value; } }
        public string Connection { get { return _innerRequest.Connection; } set { _innerRequest.Connection = value; } }
        public string Accept { get { return _innerRequest.Accept; } set { _innerRequest.Accept = value; } }
        public string Referer { get { return _innerRequest.Referer; } set { _innerRequest.Referer = value; } }
        public string UserAgent { get { return _innerRequest.UserAgent; } set { _innerRequest.UserAgent = value; } }
        public string Expect { get { return _innerRequest.Expect; } set { _innerRequest.Expect = value; } }
        public DateTime IfModifiedSince { get { return _innerRequest.IfModifiedSince; } set { _innerRequest.IfModifiedSince = value; } }
        public DateTime Date { get { return _innerRequest.Date; } set { _innerRequest.Date = value; } }
        public RequestCachePolicy CachePolicy { get { return _innerRequest.CachePolicy; } set { _innerRequest.CachePolicy = value; } }
        public AuthenticationLevel AuthenticationLevel { get { return _innerRequest.AuthenticationLevel; } set { _innerRequest.AuthenticationLevel = value; } }
        public TokenImpersonationLevel ImpersonationLevel { get { return _innerRequest.ImpersonationLevel; } set { _innerRequest.ImpersonationLevel = value; } }

        #endregion

        #region Methods

        public IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
        {
            return _innerRequest.BeginGetRequestStream(callback, state);
        }

        public Stream EndGetRequestStream(IAsyncResult asyncResult)
        {
            return _innerRequest.EndGetRequestStream(asyncResult);
        }

        public Stream EndGetRequestStream(IAsyncResult asyncResult, out TransportContext context)
        {
            return _innerRequest.EndGetRequestStream(asyncResult, out context);
        }

        public Stream GetRequestStream()
        {
            return _innerRequest.GetRequestStream();
        }

        public Stream GetRequestStream(out TransportContext context)
        {
            return _innerRequest.GetRequestStream(out context);
        }

        public IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
        {
            return _innerRequest.BeginGetResponse(callback, state);
        }

        public IHttpWebResponse EndGetResponse(IAsyncResult asyncResult)
        {
            return new HttpWebResponseWrapper((HttpWebResponse)_innerRequest.EndGetResponse(asyncResult));
        }

        public IHttpWebResponse GetResponse()
        {
            return new HttpWebResponseWrapper((HttpWebResponse)_innerRequest.GetResponse());
        }

        public void Abort()
        {
            _innerRequest.Abort();
        }

        public void AddRange(int @from, int to)
        {
            _innerRequest.AddRange(@from, to);
        }

        public void AddRange(long @from, long to)
        {
            _innerRequest.AddRange(@from, to);
        }

        public void AddRange(int range)
        {
            _innerRequest.AddRange(range);
        }

        public void AddRange(long range)
        {
            _innerRequest.AddRange(range);
        }

        public void AddRange(string rangeSpecifier, int @from, int to)
        {
            _innerRequest.AddRange(rangeSpecifier, @from, to);
        }

        public void AddRange(string rangeSpecifier, long @from, long to)
        {
            _innerRequest.AddRange(rangeSpecifier, @from, to);
        }

        public void AddRange(string rangeSpecifier, int range)
        {
            _innerRequest.AddRange(rangeSpecifier, range);
        }

        public void AddRange(string rangeSpecifier, long range)
        {
            _innerRequest.AddRange(rangeSpecifier, range);
        }

        public object GetLifetimeService()
        {
            return _innerRequest.GetLifetimeService();
        }

        public object InitializeLifetimeService()
        {
            return _innerRequest.InitializeLifetimeService();
        }

        public ObjRef CreateObjRef(Type requestedType)
        {
            return _innerRequest.CreateObjRef(requestedType);
        }

        #endregion
    }
}