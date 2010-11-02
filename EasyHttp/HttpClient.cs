using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using EasyHttp;
using JsonFx.Serialization;
using JsonFx.Serialization.Providers;
using System.Linq;
using SerializationException = System.Runtime.Serialization.SerializationException;

namespace EasyHttp
{
    public class HttpClient
    {

        HttpMethod _method;

        string _uri;
        string _contentType = "text/plain";
        object _data;
        string _accept = "text/html;application/xml";
        string _username;
        string _password;
        string _userAgent;


        public HttpResponse Response { get; private set; }
        public HttpRequest Request { get; private set;  }
        public bool ThrowExceptionOnHttpError { get; set; }

        public HttpClient()
        {
            _userAgent = String.Format("EasyHttp HttpClient v{0}", Assembly.GetAssembly(typeof (HttpClient)).GetName().Version);
            ThrowExceptionOnHttpError = true;
        }

        public HttpClient(string userAgent): this()
        {
            _userAgent = userAgent;
        }
        
        
        public HttpClient WithBasicAuthentication(string username, string password)
        {
            _username = username;
            _password = password;
            return this;
        }

        

        public HttpResponse Get(string uri)
        {
            _uri = uri;
            _method = HttpMethod.GET;
            ProcessRequest();
            return Response;
        }

        public void Post(string uri, object data, string contentType)
        {
            _uri = uri;
            _method = HttpMethod.POST;
            _contentType = contentType;
            _data = data;
            ProcessRequest();
        }

      

        public void Put(string uri, object data, string contentType)
        {
            _uri = uri;
            _contentType = contentType;
            _method = HttpMethod.PUT;
            _data = data;
            ProcessRequest();
        }

        public void Delete(string uri)
        {
            _uri = uri;
            _method = HttpMethod.DELETE;
            ProcessRequest();
        }

        public HttpClient WithAccept(string accept)
        {
            _accept = accept;
            return this;
        }

  
        public void Head(string uri)
        {
            _uri = uri;
            _method = HttpMethod.HEAD;
            ProcessRequest();
        }

        void ProcessRequest()
        {
            Request = new HttpRequest
                      {
                          ContentType = _contentType,
                          Accept = _accept,
                          Method = _method,
                          Data = _data,
                          Uri = _uri,
                          UserAgent = _userAgent
                      };

            Request.SetBasicAuthentication(_username, _password);

            Response = Request.MakeRequest();



            if (ThrowExceptionOnHttpError && IsHttpError(Response.StatusCode))
            {
                throw new HttpException(Response.StatusCode, Response.StatusDescription);
            }
        }

        static bool IsHttpError(HttpStatusCode statusCode)
        {
            var num = (int) statusCode;

            return (num/100 == 4 || num/100 == 5);
        }
    }

   
}