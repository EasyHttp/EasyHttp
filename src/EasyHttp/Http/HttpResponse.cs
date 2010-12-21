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
using System.IO;
using System.Net;
using EasyHttp.Codecs;

namespace EasyHttp.Http
{
    public class HttpResponse
    {
        readonly ICodec _codec;
        HttpWebResponse _response;

        public string ContentType { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
        public string StatusDescription { get; private set; }
        public CookieCollection Cookie { get; private set; }
        public int Age { get; private set; }
        public HttpMethod[] Allow { get; private set; }
        public CacheControl CacheControl { get; private set; }
        public string ContentEncoding { get; private set; }
        public string ContentLanguage { get; private set; }
        public long ContentLength { get; private set; }
        public string ContentLocation { get; private set; }
        
        // TODO :This should be files
        public string ContentDisposition { get; private set; }
        
        public DateTime Date { get; private set; }
        public string ETag { get; private set; }
        public DateTime Expires { get; private set; }
        public DateTime LastModified { get; private set; }
        public string Location { get; private set; }
        public CacheControl Pragma { get; private set; }
        public string Server { get; private set; }
        public WebHeaderCollection RawHeaders { get; private set; }

        

        
        public dynamic DynamicBody
        {
            get { return _codec.DecodeToDynamic(RawText, ContentType); }
        }

        public string RawText { get; set; }


        public T StaticBody<T>()
        {
            return _codec.DecodeToStatic<T>(RawText, ContentType);
        }



        public HttpResponse(ICodec codec)
        {
            _codec = codec;
     
        }

        public void GetResponse(HttpWebRequest request)
        {
            try
            {
                _response = (HttpWebResponse)request.GetResponse();

                GetHeaders();

                using (var stream = _response.GetResponseStream())
                {
                    
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            RawText = reader.ReadToEnd();
                        }
                    }
                }

            }
            catch (WebException webException)
            {
                var respone = (HttpWebResponse)webException.Response;

                StatusCode = respone.StatusCode;
                StatusDescription = respone.StatusDescription;
            }
        }

        void GetHeaders()
        {
            ContentType = _response.ContentType;
            StatusCode = _response.StatusCode;
            StatusDescription = _response.StatusDescription;
            Cookie = _response.Cookies;
            ContentEncoding = _response.ContentEncoding;
            ContentLength = _response.ContentLength;
            Date = DateTime.Now;
            LastModified = _response.LastModified;
            Server = _response.Server;

            if (!String.IsNullOrEmpty(_response.GetResponseHeader("Age")))
            {
                Age = Convert.ToInt32(_response.GetResponseHeader("Age"));
            }

            ContentLanguage = _response.GetResponseHeader("Content-Language");
            ContentLocation = _response.GetResponseHeader("Content-Location");
            ContentDisposition = _response.GetResponseHeader("Content-Disposition");
            ETag = _response.GetResponseHeader("ETag");
            Location = _response.GetResponseHeader("Location");
                
            if (!String.IsNullOrEmpty(_response.GetResponseHeader("Expires")))
            {
                DateTime expires; 
                if (DateTime.TryParse(_response.GetResponseHeader("Expires"), out expires))
                {
                    Expires = expires;
                }
            }

            // TODO: Finish this.
            //   public HttpMethod Allow { get; private set; }
            //   public CacheControl CacheControl { get; private set; }
            //   public CacheControl Pragma { get; private set; }


            RawHeaders = _response.Headers;
        }
    }
}