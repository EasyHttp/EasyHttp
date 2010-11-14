using EasyHttp.Http;

namespace EasyHttp.Infrastructure
{
    public interface ILog
    {
        void LogRequest(HttpRequest httpRequest);
        void LogResponse(HttpResponse httpRequest);
    }
}
