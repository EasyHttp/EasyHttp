using System;
using System.Net;
using System.Reflection;

namespace EasyHttp
{
    public class HttpClient
    {
        readonly ICodec _codec;

        string _userAgent = String.Format("EasyHttp HttpClient v{0}",
                                      Assembly.GetAssembly(typeof(HttpClient)).GetName().Version);


        public string UserAgent
        {
            get { return _userAgent; }
            set { _userAgent = value; }
        }

        public bool ThrowExceptionOnHttpError { get; set; }

        string _accept = "text/html;application/xml";
        string _contentType = "text/plain";
        object _data;
        HttpMethod _method;
        string _password;
        string _uri;
        string _username;

        public HttpClient()
        {
            _codec = new DefaultCodec();
        }

        public HttpClient(ICodec codec)
        {
            _codec = codec;
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
            Request = new HttpRequest(_codec)
                      {
                          ContentType = _contentType,
                          Accept = _accept,
                          Method = _method,
                          Data = _data,
                          Uri = _uri,
                          UserAgent = UserAgent
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