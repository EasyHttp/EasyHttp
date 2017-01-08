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
using System.Text;
using EasyHttp.Codecs;
using EasyHttp.Configuration;
using EasyHttp.Http.Abstractions;

namespace EasyHttp.Http
{
    public class HttpResponse
    {
        readonly IDecoder _decoder;
        IHttpWebResponse _response;

        public virtual string CharacterSet { get; private set; }
        public virtual string ContentType { get; private set; }
        public virtual HttpStatusCode StatusCode { get; private set; }
        public virtual string StatusDescription { get; private set; }
        public virtual CookieCollection Cookies { get; private set; }
        public virtual int Age { get; private set; }
        public virtual HttpMethod[] Allow { get; private set; }
        public virtual CacheControl CacheControl { get; private set; }
        public virtual string ContentEncoding { get; private set; }
        public virtual string ContentLanguage { get; private set; }
        public virtual long ContentLength { get; private set; }
        public virtual string ContentLocation { get; private set; }

        // TODO :This should be files
        public virtual string ContentDisposition { get; private set; }

        public virtual DateTime Date { get; private set; }
        public virtual string ETag { get; private set; }
        public virtual DateTime Expires { get; private set; }
        public virtual DateTime LastModified { get; private set; }
        public virtual string Location { get; private set; }
        public virtual CacheControl Pragma { get; private set; }
        public virtual string Server { get; private set; }
        public virtual WebHeaderCollection RawHeaders { get; private set; }
        public virtual Stream ResponseStream { get { return _response.GetResponseStream(); }
        }


        public virtual dynamic DynamicBody
        {
            get { return _decoder.DecodeToDynamic(RawText, ContentType); }
        }

        public virtual string RawText { get; private set; }

        public virtual T StaticBody<T>(string overrideContentType = null)
        {
            if (overrideContentType != null)
            {
                return _decoder.DecodeToStatic<T>(RawText, overrideContentType);
            }
            return _decoder.DecodeToStatic<T>(RawText, ContentType);
        }

        public HttpResponse() : this(null)
        {
        }

        public HttpResponse(IDecoder decoder)
        {
            _decoder = decoder ?? new DefaultEncoderDecoderConfiguration().GetDecoder();
        }



        public virtual void GetResponse(IHttpWebRequest request, string filename, bool streamResponse)
        {
            try
            {
                _response = request.GetResponse();

            }
            catch (WebException webException)
            {
                if (webException.Response == null)
                {
                    throw;
                }
                _response = new HttpWebResponseWrapper((HttpWebResponse) webException.Response);

            }

            GetHeaders();

	        if (streamResponse) return;

	        using (var stream = _response.GetResponseStream())
	        {
		        if (stream == null) return;

		        if (!string.IsNullOrEmpty(filename))
		        {
			        using (var filestream = new FileStream(filename, FileMode.CreateNew))
			        {
				        int count;
				        var buffer = new byte[8192];

				        while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
				        {
					        filestream.Write(buffer, 0, count);
				        }
			        }
		        }
		        else
		        {
                    var encoding = string.IsNullOrEmpty(CharacterSet) ? Encoding.UTF8 : Encoding.GetEncoding(CharacterSet);
                    using (var reader = new StreamReader(stream, encoding))
			        {
				        RawText = reader.ReadToEnd();
			        }
		        }
	        }
        }

        void GetHeaders()
        {
            CharacterSet = _response.CharacterSet;
            ContentType = _response.ContentType;
            StatusCode = _response.StatusCode;
            StatusDescription = _response.StatusDescription;
            Cookies = _response.Cookies;
            ContentEncoding = _response.ContentEncoding;
            ContentLength = _response.ContentLength;
            Date = DateTime.Now;
            LastModified = _response.LastModified;
            Server = _response.Server;

            if (!String.IsNullOrEmpty(GetHeader("Age")))
            {
                Age = Convert.ToInt32(GetHeader("Age"));
            }

            ContentLanguage = GetHeader("Content-Language");
            ContentLocation = GetHeader("Content-Location");
            ContentDisposition = GetHeader("Content-Disposition");
            ETag = GetHeader("ETag");
            Location = GetHeader("Location");

            if (!String.IsNullOrEmpty(GetHeader("Expires")))
            {
                DateTime expires;
                if (DateTime.TryParse(GetHeader("Expires"), out expires))
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

        string GetHeader(string header)
        {
            var headerValue = _response.GetResponseHeader(header);

            return headerValue.Replace("\"", "");
        }
    }
}