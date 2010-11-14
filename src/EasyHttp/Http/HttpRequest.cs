using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Cache;
using System.Reflection;
using EasyHttp.Codecs;

namespace EasyHttp.Http
{
    public class HttpRequest
    {

        public string Accept { get ; set; }
        public string AcceptCharSet { get; set; }
        public string AcceptEncoding { get; set; }
        public string AcceptLanguage { get; set; }
        public bool KeepAlive { get; set; }
        public string ContentLength { get; private set; }
        public string ContentType { get; set; }
        public CookieCollection Cookies { get; set; }
        public DateTime Date { get; set; }
        public bool Expect { get; set; }
        public string From { get; set; }
        public string Host { get; set; }
        public string IfMatch { get; set; }
        public DateTime IfModifiedSince { get; set; }
        public string IfRange { get; set; }
        public int MaxForwards { get; set; }
        public string Referer { get; set; }
        public int Range { get; set; }
        public string UserAgent { get; set; }

        public IDictionary<string, string> RawHeaders { get; private set; }
        

        public HttpMethod Method { get; set; }

        public object Data { get; set; }

        public string Uri { get; set; }

        HttpWebRequest httpWebRequest;

        readonly ICodec _codec;
        string _username;
        string _password;
        
        HttpRequestCachePolicy _cachePolicy;
        
        public HttpRequest(ICodec codec)
        {
            RawHeaders = new Dictionary<string, string>();
            
            UserAgent = String.Format("EasyHttp HttpClient v{0}",
                                       Assembly.GetAssembly(typeof(HttpClient)).GetName().Version);

            Accept = String.Join(";", HttpContentTypes.TextHtml, HttpContentTypes.ApplicationXml,
                                 HttpContentTypes.ApplicationJson);
            _codec = codec;


        }


        public void SetBasicAuthentication(string username, string password)
        {
            _username = username;
            _password = password;
        }

        void SetupHeader()
        {
            httpWebRequest.ContentType = ContentType;
            httpWebRequest.Accept = Accept;
            httpWebRequest.Method = Method.ToString();
            httpWebRequest.UserAgent = UserAgent;
            httpWebRequest.Referer = Referer;
            httpWebRequest.CachePolicy = _cachePolicy;
            httpWebRequest.KeepAlive = KeepAlive;

            if (Cookies != null )
            {
                httpWebRequest.CookieContainer = new CookieContainer(Cookies.Count);
                
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

        }

        public void AddExtraHeader(string header, object value)
        {
            if (value != null)
            {
                httpWebRequest.Headers.Add(String.Format("{0}: {1}", header, value));
            }
        }

        void SetupBody()
        {

            if (Data == null)
            {
                return;
            }

            var bytes = _codec.Encode(Data, ContentType);

            var requestStream = httpWebRequest.GetRequestStream();
            
            requestStream.Write(bytes, 0, bytes.Length);
                
            requestStream.Close();
        }


        public HttpResponse MakeRequest()
        {
            httpWebRequest = (HttpWebRequest) WebRequest.Create(Uri);

            SetupHeader();
            
            SetupBody();


            var response = new HttpResponse(_codec);

            response.GetResponse(httpWebRequest);

            return response;
        }

        void SetupAuthentication()
        {
            if (!String.IsNullOrEmpty(_username) && !String.IsNullOrEmpty(_password))
            {
                var networkCredential = new NetworkCredential(_username, _password);

                httpWebRequest.Credentials = networkCredential;
            }
        }


        public void SetCacheControlToNoCache()
        {
            _cachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
        }

        public void SetCacheControlWithMaxAge(TimeSpan maxAge)
        {
            _cachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, maxAge);
        }

        public void SetCacheControlWithMaxAgeAndMaxStale(TimeSpan maxAge, TimeSpan maxStale)
        {
            _cachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAgeAndMaxStale, maxAge, maxStale);
        }

        public void SetCacheControlWithMaxAgeAndMinFresh(TimeSpan maxAge, TimeSpan minFresh)
        {
            _cachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAgeAndMinFresh, minFresh);
        }

    }
}