using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Runtime.Remoting;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using EasyHttp.Http.Abstractions;

namespace EasyHttp.Http.Injection
{
    internal class StubbedHttpWebRequest : IHttpWebRequest
    {
        private readonly HttpRequestInterception _matchingInterceptor;

        public StubbedHttpWebRequest(HttpRequestInterception matchingInterceptor)
        {
            _matchingInterceptor = matchingInterceptor;
        }

        public bool AllowAutoRedirect { get; set; }
        public bool AllowWriteStreamBuffering { get; set; }
        public bool HaveResponse { get; private set; }
        public bool KeepAlive { get; set; }
        public bool Pipelined { get; set; }
        public bool PreAuthenticate { get; set; }
        public bool UnsafeAuthenticatedConnectionSharing { get; set; }
        public bool SendChunked { get; set; }
        public DecompressionMethods AutomaticDecompression { get; set; }
        public int MaximumResponseHeadersLength { get; set; }
        public X509CertificateCollection ClientCertificates { get; set; }
        public CookieContainer CookieContainer { get; set; }
        public bool SupportsCookieContainer { get; private set; }
        public Uri RequestUri { get; private set; }
        public long ContentLength { get; set; }
        public int Timeout { get; set; }
        public int ReadWriteTimeout { get; set; }
        public Uri Address { get; private set; }
        public HttpContinueDelegate ContinueDelegate { get; set; }
        public ServicePoint ServicePoint { get; private set; }
        public string Host { get; set; }
        public int MaximumAutomaticRedirections { get; set; }
        public string Method { get; set; }
        public ICredentials Credentials { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public string ConnectionGroupName { get; set; }
        public WebHeaderCollection Headers { get; set; }
        public IWebProxy Proxy { get; set; }
        public Version ProtocolVersion { get; set; }
        public string ContentType { get; set; }
        public string MediaType { get; set; }
        public string TransferEncoding { get; set; }
        public string Connection { get; set; }
        public string Accept { get; set; }
        public string Referer { get; set; }
        public string UserAgent { get; set; }
        public string Expect { get; set; }
        public DateTime IfModifiedSince { get; set; }
        public DateTime Date { get; set; }
        public RequestCachePolicy CachePolicy { get; set; }
        public AuthenticationLevel AuthenticationLevel { get; set; }
        public TokenImpersonationLevel ImpersonationLevel { get; set; }

        public IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public Stream EndGetRequestStream(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }

        public Stream EndGetRequestStream(IAsyncResult asyncResult, out TransportContext context)
        {
            throw new NotImplementedException();
        }

        public Stream GetRequestStream()
        {
            throw new NotImplementedException();
        }

        public Stream GetRequestStream(out TransportContext context)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public IHttpWebResponse EndGetResponse(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }

        public IHttpWebResponse GetResponse()
        {
            return _matchingInterceptor.GetInjectedResponse();
        }

        public void Abort()
        {
            throw new NotImplementedException();
        }

        public void AddRange(int @from, int to)
        {
            throw new NotImplementedException();
        }

        public void AddRange(long @from, long to)
        {
            throw new NotImplementedException();
        }

        public void AddRange(int range)
        {
            throw new NotImplementedException();
        }

        public void AddRange(long range)
        {
            throw new NotImplementedException();
        }

        public void AddRange(string rangeSpecifier, int @from, int to)
        {
            throw new NotImplementedException();
        }

        public void AddRange(string rangeSpecifier, long @from, long to)
        {
            throw new NotImplementedException();
        }

        public void AddRange(string rangeSpecifier, int range)
        {
            throw new NotImplementedException();
        }

        public void AddRange(string rangeSpecifier, long range)
        {
            throw new NotImplementedException();
        }

        public object GetLifetimeService()
        {
            throw new NotImplementedException();
        }

        public object InitializeLifetimeService()
        {
            throw new NotImplementedException();
        }

        public ObjRef CreateObjRef(Type requestedType)
        {
            throw new NotImplementedException();
        }
    }
}