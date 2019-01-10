using System.IO;
using EasyHttp.Http;

namespace EasyHttp.Infrastructure
{
    public abstract class MultiPartFileDataAbstraction
    {
        public string FieldName { get; set; }
        public string ContentType { get; set; }
        public string ContentTransferEncoding { get; set; }

        protected MultiPartFileDataAbstraction()
        {
            ContentTransferEncoding = HttpContentTransferEncoding.Binary;
        }

        public abstract Stream GetStream();
        public abstract long GetLength();
        public abstract string GetFilenameForDisposition();
    }
}