using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using JsonFx.Serialization;
using JsonFx.Serialization.Providers;
using SerializationException = System.Runtime.Serialization.SerializationException;

namespace EasyHttp
{
    public class EasyHttp
    {
        readonly Request request;

        public Response Response { get; private set; }
        public Request Request { get { return request; } }

        public EasyHttp()
        {
            request = new Request();
        }

        public Response Get(string uri)
        {
            Response = request.MakeRequest(uri, HttpMethod.GET);

            return Response;
        }

        public void Post(string uri, object data)
        {
            Response = request.MakeRequest(uri, HttpMethod.POST, data);
        }

      

        public void Put(string uri, object data)
        {
            Response = request.MakeRequest(uri, HttpMethod.PUT, data);
        }

        public void Delete(string uri)
        {
            Response = request.MakeRequest(uri, HttpMethod.DELETE);
        }

        // TODO: Fix this up. Shouldn't be here
        public EasyHttp WithContentType(string contentType)
        {
            request.Header.ContentType = contentType;
            return this;
        }

        // TODO: Fix this up. Shouldn't be here
        public EasyHttp WithAccept(string accept)
        {
            request.Header.Accept = accept;
            return this;
        }

        public void Head(string uri)
        {
            Response = request.MakeRequest(uri, HttpMethod.HEAD);
        }
    }
}