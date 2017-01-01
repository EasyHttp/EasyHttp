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
using System.Security.Cryptography.X509Certificates;
using EasyHttp.Codecs;
using EasyHttp.Configuration;
using EasyHttp.Infrastructure;

namespace EasyHttp.Http
{
    public class HttpClient
    {
        readonly string _baseUri;
        readonly IEncoder _encoder;
        readonly IDecoder _decoder;
        readonly UriComposer _uriComposer;
        private bool _shouldRemoveAtSign = true;
        private readonly Func<string, HttpResponse> _getResponse;

        public virtual bool LoggingEnabled { get; set; }
        public virtual bool ThrowExceptionOnHttpError { get; set; }
        public virtual bool StreamResponse { get; set; }

        public virtual bool ShouldRemoveAtSign
        {
            get { return _shouldRemoveAtSign; }
            set
            {
                _shouldRemoveAtSign = value;
                _decoder.ShouldRemoveAtSign = value;
            }
        }

        public HttpClient() : this(null)
        {
        }

        public HttpClient(Func<string,HttpResponse> getResponse = null):this(new DefaultEncoderDecoderConfiguration(), getResponse)
        {
        }


        public HttpClient(IEncoderDecoderConfiguration encoderDecoderConfiguration, Func<string, HttpResponse> getResponse = null)
        {
            _encoder = encoderDecoderConfiguration.GetEncoder();
            _decoder = encoderDecoderConfiguration.GetDecoder();
            _decoder.ShouldRemoveAtSign = _shouldRemoveAtSign;
            _uriComposer = new UriComposer();

            Request = new HttpRequest(_encoder);
            _getResponse = getResponse ?? GetResponse;
        }

        public HttpClient(string baseUri, Func<string,HttpResponse> getResponse = null): this(new DefaultEncoderDecoderConfiguration(), getResponse)
        {
            _baseUri = baseUri;
        }

        public virtual HttpResponse Response { get; private set; }
        public virtual HttpRequest Request { get; private set; }

        void InitRequest(string uri, HttpMethod method, object query)
        {
            Request.Uri = _uriComposer.Compose(_baseUri, uri, query, Request.ParametersAsSegments);
            Request.Data = null;
            Request.PutFilename = String.Empty;
            Request.Expect = false;
            Request.KeepAlive = true;
            Request.MultiPartFormData = null;
            Request.MultiPartFileData = null;
            Request.ContentEncoding = null;
            Request.Method = method;
        }


        public virtual HttpResponse GetAsFile(string uri, string filename)
        {
            InitRequest(uri, HttpMethod.GET, null);
            return ProcessRequest(filename);
        }

        public virtual HttpResponse Get(string uri, object query = null)
        {
            InitRequest(uri, HttpMethod.GET, query);
            return ProcessRequest();
        }

        public virtual HttpResponse Options(string uri)
        {
            InitRequest(uri, HttpMethod.OPTIONS, null);
            return ProcessRequest();
        }

        public virtual HttpResponse Post(string uri, object data, string contentType, object query = null)
        {
            InitRequest(uri, HttpMethod.POST, query);
            InitData(data, contentType);
            return ProcessRequest();
        }

        public virtual HttpResponse Patch(string uri, object data, string contentType, object query = null)
        {
            InitRequest(uri, HttpMethod.PATCH, query);
            InitData(data, contentType);
            return ProcessRequest();
        }

        public virtual HttpResponse Post(string uri, IDictionary<string, object> formData, IList<FileData> files, object query = null)
        {
            InitRequest(uri, HttpMethod.POST, query);
            Request.MultiPartFormData = formData;
            Request.MultiPartFileData = files;
            Request.KeepAlive = true;
            return ProcessRequest();
        }

        public virtual HttpResponse Put(string uri, object data, string contentType, object query = null)
        {
            InitRequest(uri, HttpMethod.PUT, query);
            InitData(data, contentType);
            return ProcessRequest();
        }

        void InitData(object data, string contentType)
        {
            if (data != null)
            {
                Request.ContentType = contentType;
                Request.Data = data;
            }
        }

        public virtual HttpResponse Delete(string uri, object query = null)
        {
            InitRequest(uri, HttpMethod.DELETE, query);
            return ProcessRequest();
        }


        public virtual HttpResponse Head(string uri, object query = null)
        {
            InitRequest(uri, HttpMethod.HEAD, query);
            return ProcessRequest();
        }

        public virtual HttpResponse PutFile(string uri, string filename, string contentType)
        {
            InitRequest(uri, HttpMethod.PUT, null);
            Request.ContentType = contentType;
            Request.PutFilename = filename;
            Request.Expect = true;
            Request.KeepAlive = true;
            return ProcessRequest();
        }

        HttpResponse ProcessRequest(string filename = "")
        {
            Response = _getResponse(filename);

            if (ThrowExceptionOnHttpError && IsHttpError())
            {
                throw new HttpException(Response.StatusCode, Response.StatusDescription);
            }
            return Response;
        }

        private HttpResponse GetResponse(string filename)
        {
            var httpWebRequest = Request.PrepareRequest();

            var response = new HttpResponse(_decoder);

            response.GetResponse(httpWebRequest, filename, StreamResponse);

            return response;
        }

        public virtual void AddClientCertificates(X509CertificateCollection certificates)
        {
            if(certificates == null || certificates.Count == 0)
                return;

            Request.ClientCertificates.AddRange(certificates);
        }

        bool IsHttpError()
        {
            var num = (int) Response.StatusCode / 100;

            return (num == 4 || num == 5);
        }

    }
}