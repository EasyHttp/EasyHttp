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
    public interface IHttpWebRequest
    {
        bool AllowAutoRedirect { get; set; }
        bool AllowWriteStreamBuffering { get; set; }
        bool HaveResponse { get; }
        bool KeepAlive { get; set; }
        bool Pipelined { get; set; }
        bool PreAuthenticate { get; set; }
        bool UnsafeAuthenticatedConnectionSharing { get; set; }
        bool SendChunked { get; set; }
        DecompressionMethods AutomaticDecompression { get; set; }
        int MaximumResponseHeadersLength { get; set; }
        X509CertificateCollection ClientCertificates { get; set; }
        CookieContainer CookieContainer { get; set; }
        bool SupportsCookieContainer { get; }
        Uri RequestUri { get; }
        long ContentLength { get; set; }
        int Timeout { get; set; }
        int ReadWriteTimeout { get; set; }
        Uri Address { get; }
        HttpContinueDelegate ContinueDelegate { get; set; }
        ServicePoint ServicePoint { get; }
        string Host { get; set; }
        int MaximumAutomaticRedirections { get; set; }
        string Method { get; set; }
        ICredentials Credentials { get; set; }
        bool UseDefaultCredentials { get; set; }
        string ConnectionGroupName { get; set; }
        WebHeaderCollection Headers { get; set; }
        IWebProxy Proxy { get; set; }
        Version ProtocolVersion { get; set; }
        String ContentType { get; set; }
        string MediaType { get; set; }
        string TransferEncoding { get; set; }
        string Connection { get; set; }
        string Accept { get; set; }
        string Referer { get; set; }
        string UserAgent { get; set; }
        string Expect { get; set; }
        DateTime IfModifiedSince { get; set; }
        DateTime Date { get; set; }
        RequestCachePolicy CachePolicy { get; set; }
        AuthenticationLevel AuthenticationLevel { get; set; }
        TokenImpersonationLevel ImpersonationLevel { get; set; }
        IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state);
        Stream EndGetRequestStream(IAsyncResult asyncResult);
        Stream EndGetRequestStream(IAsyncResult asyncResult, out TransportContext context);
        Stream GetRequestStream();
        Stream GetRequestStream(out TransportContext context);
        IAsyncResult BeginGetResponse(AsyncCallback callback, object state);
        IHttpWebResponse EndGetResponse(IAsyncResult asyncResult);
        IHttpWebResponse GetResponse();
        void Abort();
        void AddRange(int from, int to);
        void AddRange(long from, long to);
        void AddRange(int range);
        void AddRange(long range);
        void AddRange(string rangeSpecifier, int from, int to);
        void AddRange(string rangeSpecifier, long from, long to);
        void AddRange(string rangeSpecifier, int range);
        void AddRange(string rangeSpecifier, long range);
        object GetLifetimeService();
        object InitializeLifetimeService();
        ObjRef CreateObjRef(Type requestedType);
    }
}