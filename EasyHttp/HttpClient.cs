using System;
using System.Net;
using System.Reflection;
using StructureMap;

namespace EasyHttp
{
    public class HttpClient
    {
        readonly ICodec _codec;


        public string UserAgent { get; set; }

        public bool ThrowExceptionOnHttpError { get; set; }

        string _accept = String.Format("{0};{1};{2}", HttpContentTypes.TextHtml, HttpContentTypes.ApplicationXml,
                                       HttpContentTypes.ApplicationJson); 
        
        string _password;
        string _username;

        public HttpClient()
        {
            BootStrapper.InitStructureMap();

            UserAgent = String.Format("EasyHttp HttpClient v{0}",
                                      Assembly.GetAssembly(typeof (HttpClient)).GetName().Version);

            _codec = ObjectFactory.GetInstance<ICodec>();

            var tasks = ObjectFactory.GetAllInstances<IStartupTask>();
            
            foreach(var task in tasks)
            {
                task.Execute();
            }
        }

 
        public HttpResponse Response { get; private set; }
        public HttpRequest Request { get; private set; }


        public HttpClient WithBasicAuthentication(string username, string password)
        {
            _username = username;
            _password = password;
            return this;
        }


        public HttpResponse Get(string uri)
        {
            ProcessRequest(new HttpRequest(_codec)
                           {
                               Method = HttpMethod.GET,
                               Uri = uri,
                           });
            return Response;
        }

        public void Post(string uri, object data, string contentType)
        {
            ProcessRequest(new HttpRequest(_codec)
                           {
                               ContentType = contentType,
                               Method = HttpMethod.POST,
                               Data = data,
                               Uri = uri,
                           });
        }


        public void Put(string uri, object data, string contentType)
        {
            ProcessRequest(new HttpRequest(_codec)
                           {
                               ContentType = contentType,
                               Method = HttpMethod.PUT,
                               Data = data,
                               Uri = uri,
                           });
        }

        public void Delete(string uri)
        {
            ProcessRequest(new HttpRequest(_codec)
                           {
                               Method = HttpMethod.DELETE,
                               Uri = uri,
                           });
        }

        public HttpClient WithAccept(params string []accepts)
        {
            _accept = string.Join(";", accepts);
            return this;
        }


        public void Head(string uri)
        {
            ProcessRequest(new HttpRequest(_codec)
                           {
                               Method = HttpMethod.HEAD,
                               Uri = uri,
                           });
        }

        void ProcessRequest(HttpRequest httpRequest)
        {
            httpRequest.UserAgent = UserAgent;
            httpRequest.Accept = _accept;

            httpRequest.SetBasicAuthentication(_username, _password);


            Request = httpRequest;

            Response = httpRequest.MakeRequest();


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