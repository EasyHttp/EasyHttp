using System;

namespace EasyHttp.Http
{
    public interface IRequestProcessor
    {
        HttpResponse ProcessRequest();
    }

    public class FileRequestProcessor : IRequestProcessor
    {
        readonly HttpRequest _request;
        readonly string _filename;

        public FileRequestProcessor(HttpRequest request, string filename)
        {
            _request = request;
            _filename = filename;
        }

        public HttpResponse ProcessRequest()
        {
            return _request.MakeFileRequest(_filename);
        }
    }

    public class BodyRequestProcessor : IRequestProcessor
    {
        readonly HttpRequest _request;

        public BodyRequestProcessor(HttpRequest request)
        {
            _request = request;
        }

        public HttpResponse ProcessRequest()
        {
            return _request.MakeRequest();
        }
    }
}