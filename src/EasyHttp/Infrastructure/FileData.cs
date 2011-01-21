using EasyHttp.Http;

namespace EasyHttp.Infrastructure
{
    public class FileData
    {
        public string Filename { get; set; }
        public string ContentType { get; set; }
        public string ContentEncoding { get; set; }

        public FileData()
        {
            ContentEncoding = HttpContentEncoding.Binary;
        }
    }
}