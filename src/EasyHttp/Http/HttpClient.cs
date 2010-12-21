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

using EasyHttp.Codecs;
using EasyHttp.Configuration;
using EasyHttp.Infrastructure;
using StructureMap;

namespace EasyHttp.Http
{
    public class HttpClient
    {
        readonly ICodec _codec;
        readonly ILog _log;

        public bool LoggingEnabled { get; set; }
        public bool ThrowExceptionOnHttpError { get; set; }
      
        
        public HttpClient():this(new DefaultContainerConfiguration())
        {

        }
      
        public HttpClient(IContainerConfiguration containerConfiguration)
        {
            var registry = containerConfiguration.InitializeContainer();

            ObjectFactory.Initialize(
                x => x.AddRegistry(registry));

            _codec = ObjectFactory.GetInstance<ICodec>();

            _log = ObjectFactory.TryGetInstance<ILog>();

            Request = new HttpRequest(_codec);
        }

        public HttpResponse Response { get; private set; }
        public HttpRequest Request { get; private set; }


        public HttpResponse Get(string uri)
        {
            Request.Method = HttpMethod.GET;
            Request.Uri = uri;
            ProcessRequest();

            return Response;
        }

        public void Post(string uri, object data, string contentType)
        {
            Request.ContentType = contentType;
            Request.Method = HttpMethod.POST;
            Request.Data = data;
            Request.Uri = uri;
 
            ProcessRequest();
        }


        public void Put(string uri, object data, string contentType)
        {
            Request.ContentType = contentType;
            Request.Method = HttpMethod.PUT;
            Request.Data = data;
            Request.Uri = uri;
            ProcessRequest();
        }

        public void Delete(string uri)
        {
            Request.Method = HttpMethod.DELETE;
            Request.Uri = uri;
            ProcessRequest();
        }

 
        public void Head(string uri)
        {
            Request.Method = HttpMethod.HEAD;
            Request.Uri = uri;
            ProcessRequest();
        }

        void ProcessRequest()
        {
            if (CanLog())
            {
                _log.LogRequest(Request);
            }

            Response = Request.MakeRequest();

            if (CanLog())
            {
                _log.LogResponse(Response);
            }


            if (ThrowExceptionOnHttpError && IsHttpError())
            {
                throw new HttpException(Response.StatusCode, Response.StatusDescription);
            }
        }

        bool CanLog()
        {
            if (LoggingEnabled)
            {
                if (_log == null)
                {
                    throw new ConfigurationException("Logging is enabled but no valid logger has been configured");
                }
                return true;
            }
            return false;
        }


        bool IsHttpError()
        {
            var num = (int) Response.StatusCode / 100;

            return (num == 4 || num == 5);
        }


    }

   
}