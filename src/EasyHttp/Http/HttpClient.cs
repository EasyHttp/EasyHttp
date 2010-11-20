using System;
using System.Collections.Generic;
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


        public HttpClient(IConfiguration configuration)
        {

            ObjectFactory.Initialize(
                x =>
                {
                    foreach (var registry in configuration.Registries)
                    {
                        x.AddRegistry(registry);
                    }
                });

            _codec = ObjectFactory.GetInstance<ICodec>();

            _log = ObjectFactory.TryGetInstance<ILog>();


            var tasks = ObjectFactory.GetAllInstances<IConfigurationStep>();

            foreach (var task in tasks)
            {
                task.Execute();
            }

            Request = new HttpRequest(_codec);
        }
      

        public HttpClient(): this(new DefaultConfiguration())
        {
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