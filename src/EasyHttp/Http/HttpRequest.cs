#region License
// Distributed under the BSD License
// =================================
// 
// Copyright (c) 2010, Hadi Hariri
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of Hadi Hariri nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// =============================================================
// 
// 
// Parts of this Software use JsonFX Serialization Library which is distributed under the MIT License:
// 
// Distributed under the terms of an MIT-style license:
// 
// The MIT License
// 
// Copyright (c) 2006-2009 Stephen M. McKamey
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
#endregion

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
        public string ContentEncoding { get; set; }
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
            var cookieContainer = new CookieContainer();

            httpWebRequest.CookieContainer = cookieContainer;
            httpWebRequest.ContentType = ContentType;
            httpWebRequest.Accept = Accept;
            httpWebRequest.Method = Method.ToString();
            httpWebRequest.UserAgent = UserAgent;
            httpWebRequest.Referer = Referer;
            httpWebRequest.CachePolicy = _cachePolicy;
            httpWebRequest.KeepAlive = KeepAlive;
            

            if (Cookies != null )
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


        public HttpResponse MakeRequest(string filename)
        {
         
            httpWebRequest = (HttpWebRequest) WebRequest.Create(Uri);

            SetupHeader();
            
            SetupBody();


            var response = new HttpResponse(_codec);

            response.GetResponse(httpWebRequest, filename);
            
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