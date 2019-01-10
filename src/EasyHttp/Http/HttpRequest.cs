#region License

// Distributed under the BSD License
//
// YouTrackSharp Copyright (c) 2010-2012, Hadi Hariri and Contributors
// All rights reserved.
//
//  Redistribution and use in source and binary forms, with or without
//  modification, are permitted provided that the following conditions are met:
//      * Redistributions of source code must retain the above copyright
//         notice, this list of conditions and the following disclaimer.
//      * Redistributions in binary form must reproduce the above copyright
//         notice, this list of conditions and the following disclaimer in the
//         documentation and/or other materials provided with the distribution.
//      * Neither the name of Hadi Hariri nor the
//         names of its contributors may be used to endorse or promote products
//         derived from this software without specific prior written permission.
//
//   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//   "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
//   TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
//   PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL
//   <COPYRIGHTHOLDER> BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
//   SPECIAL,EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
//   LIMITED  TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
//   DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND  ON ANY
//   THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//   (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
//   THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using EasyHttp.Codecs;
using EasyHttp.Http.Abstractions;
using EasyHttp.Infrastructure;

namespace EasyHttp.Http
{
    // TODO: This class needs cleaning up and abstracting the encoder one more level
    public class HttpRequest
    {
        readonly IEncoder _encoder;
        HttpRequestCachePolicy _cachePolicy;
        bool _forceBasicAuth;
        string _password;
        string _username;
        IHttpWebRequest httpWebRequest;
        CookieContainer cookieContainer;

        public HttpRequest(IEncoder encoder)
        {
            RawHeaders = new Dictionary<string, object>();

            ClientCertificates = new X509CertificateCollection();

            UserAgent = String.Format("EasyHttp HttpClient v{0}",
                                      Assembly.GetAssembly(typeof (HttpClient)).GetName().Version);

            Accept = String.Join(";", HttpContentTypes.TextHtml, HttpContentTypes.ApplicationXml,
                                 HttpContentTypes.ApplicationJson);
            _encoder = encoder;

            Timeout = 100000; //http://msdn.microsoft.com/en-us/library/system.net.httpwebrequest.timeout.aspx

            AllowAutoRedirect = true;
        }

        public virtual bool DisableAutomaticCompression { get; set; }
        public virtual string Accept { get; set; }
        public virtual string AcceptCharSet { get; set; }
        public virtual string AcceptEncoding { get; set; }
        public virtual string AcceptLanguage { get; set; }
        public virtual bool KeepAlive { get; set; }
        public virtual X509CertificateCollection ClientCertificates { get; set; }
        public virtual string ContentLength { get; private set; }
        public virtual string ContentType { get; set; }
        public virtual string ContentEncoding { get; set; }
        public virtual CookieCollection Cookies { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual bool Expect { get; set; }
        public virtual string From { get; set; }
        public virtual string Host { get; set; }
        public virtual string IfMatch { get; set; }
        public virtual DateTime IfModifiedSince { get; set; }
        public virtual string IfRange { get; set; }
        public virtual int MaxForwards { get; set; }
        public virtual string Referer { get; set; }
        public virtual int Range { get; set; }
        public virtual string UserAgent { get; set; }
        public virtual IDictionary<string, object> RawHeaders { get; private set; }
        public virtual HttpMethod Method { get; set; }
        public virtual object Data { get; set; }
        public virtual string Uri { get; set; }
        public virtual Stream PutStream { get; set; }
        public virtual string PutFilename { get; set; }
        public virtual IDictionary<string, object> MultiPartFormData { get; set; }
        public virtual IList<MultiPartFileDataAbstraction> MultiPartFileData { get; set; }
        public virtual int Timeout { get; set; }
        public virtual Boolean ParametersAsSegments { get; set; }

        public virtual bool ForceBasicAuth
        {
            get { return _forceBasicAuth; }
            set { _forceBasicAuth = value; }
        }

        public virtual bool PersistCookies { get; set; }
        public virtual bool AllowAutoRedirect { get; set; }

        public virtual void SetBasicAuthentication(string username, string password)
        {
            _username = username;
            _password = password;
        }

        void SetupHeader()
        {
            if(!PersistCookies || cookieContainer == null)
                cookieContainer = new CookieContainer();

            httpWebRequest.CookieContainer = cookieContainer;
            httpWebRequest.ContentType = ContentType;
            httpWebRequest.Accept = Accept;
            httpWebRequest.Method = Method.ToString();
            httpWebRequest.UserAgent = UserAgent;
            httpWebRequest.Referer = Referer;
            httpWebRequest.CachePolicy = _cachePolicy;
            httpWebRequest.KeepAlive = KeepAlive;
            httpWebRequest.AutomaticDecompression = DisableAutomaticCompression
                                                    ? DecompressionMethods.None
                                                    : DecompressionMethods.Deflate | DecompressionMethods.GZip | DecompressionMethods.None;

            ServicePointManager.Expect100Continue = Expect;
            ServicePointManager.ServerCertificateValidationCallback = AcceptAllCertifications;

            if (Timeout > 0)
            {
                httpWebRequest.Timeout = Timeout;
            }


            if (Cookies != null)
            {
                httpWebRequest.CookieContainer.Add(Cookies);
            }

            if (IfModifiedSince != DateTime.MinValue)
            {
                httpWebRequest.IfModifiedSince = IfModifiedSince;
            }


            if (Date != DateTime.MinValue)
            {
                httpWebRequest.Date = Date;
            }

            if (!String.IsNullOrEmpty(Host))
            {
                httpWebRequest.Host = Host;
            }

            if (MaxForwards != 0)
            {
                httpWebRequest.MaximumAutomaticRedirections = MaxForwards;
            }

            if (Range != 0)
            {
                httpWebRequest.AddRange(Range);
            }

            SetupAuthentication();

            AddExtraHeader("From", From);
            AddExtraHeader("Accept-CharSet", AcceptCharSet);
            AddExtraHeader("Accept-Encoding", AcceptEncoding);
            AddExtraHeader("Accept-Language", AcceptLanguage);
            AddExtraHeader("If-Match", IfMatch);
            AddExtraHeader("Content-Encoding", ContentEncoding);

            foreach (var header in RawHeaders)
            {
                httpWebRequest.Headers.Add(String.Format("{0}: {1}", header.Key, header.Value));
            }
        }

        bool AcceptAllCertifications(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            return true;
        }

        public virtual void AddExtraHeader(string header, object value)
        {
            if (value != null && !RawHeaders.ContainsKey(header))
            {
                RawHeaders.Add(header, value);
            }
        }

        void SetupBody()
        {
            if (Data != null)
            {
                SetupData();

                return;
            }

            if (PutStream != null)
            {
                SetupPutStream();
                return;
            }

            if (!String.IsNullOrEmpty(PutFilename))
            {
                SetupPutFilename();
                return;
            }

            if (MultiPartFormData != null || MultiPartFileData != null)
            {
                SetupMultiPartBody();
            }
        }

        void SetupData()
        {
            var bytes = _encoder.Encode(Data, ContentType);

            if (bytes.Length > 0)
            {
                httpWebRequest.ContentLength = bytes.Length;
            }

            var requestStream = httpWebRequest.GetRequestStream();

            requestStream.Write(bytes, 0, bytes.Length);

            requestStream.Close();
        }

        void SetupPutFilename()
        {
            using (var fileStream = new FileStream(PutFilename, FileMode.Open, FileAccess.Read))
            {
                httpWebRequest.ContentLength = fileStream.Length;

                var requestStream = httpWebRequest.GetRequestStream();

                var buffer = new byte[81982];

                int bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                while (bytesRead > 0)
                {
                    requestStream.Write(buffer, 0, bytesRead);
                    bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                }
                requestStream.Close();
            }
        }

        
        void SetupPutStream()
        {
            if (PutStream.Length > 0)
            {
                httpWebRequest.ContentLength = PutStream.Length;
            }

            var requestStream = httpWebRequest.GetRequestStream();

            var buffer = new byte[81982];

            int bytesRead = PutStream.Read(buffer, 0, buffer.Length);
            while (bytesRead > 0)
            {
                requestStream.Write(buffer, 0, bytesRead);
                bytesRead = PutStream.Read(buffer, 0, buffer.Length);
            }
            requestStream.Close();
        }



        void SetupMultiPartBody()
        {
            var multiPartStreamer = new MultiPartStreamer(MultiPartFormData, MultiPartFileData);

            httpWebRequest.ContentType = multiPartStreamer.GetContentType();
            var contentLength = multiPartStreamer.GetContentLength();

            if (contentLength > 0)
            {
                httpWebRequest.ContentLength = contentLength;
            }

            multiPartStreamer.StreamMultiPart(httpWebRequest.GetRequestStream());

        }


        public virtual IHttpWebRequest PrepareRequest()
        {
            httpWebRequest = new HttpWebRequestWrapper((HttpWebRequest) WebRequest.Create(Uri));
            httpWebRequest.AllowAutoRedirect = AllowAutoRedirect;
            SetupHeader();

            SetupBody();

            return httpWebRequest;
        }

        void SetupClientCertificates()
        {
            if (ClientCertificates == null || ClientCertificates.Count == 0)
                return;

            httpWebRequest.ClientCertificates.AddRange(ClientCertificates);
        }

        void SetupAuthentication()
        {
            SetupClientCertificates();

            if (_forceBasicAuth)
            {
                string authInfo = _username + ":" + _password;
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                httpWebRequest.Headers["Authorization"] = "Basic " + authInfo;
            }
            else
            {
                var networkCredential = new NetworkCredential(_username, _password);
                httpWebRequest.Credentials = networkCredential;
            }
        }


        public virtual void SetCacheControlToNoCache()
        {
            _cachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
        }

        public virtual void SetCacheControlWithMaxAge(TimeSpan maxAge)
        {
            _cachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, maxAge);
        }

        public virtual void SetCacheControlWithMaxAgeAndMaxStale(TimeSpan maxAge, TimeSpan maxStale)
        {
            _cachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAgeAndMaxStale, maxAge, maxStale);
        }

        public virtual void SetCacheControlWithMaxAgeAndMinFresh(TimeSpan maxAge, TimeSpan minFresh)
        {
            _cachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAgeAndMinFresh, maxAge, minFresh);
        }
    }
}