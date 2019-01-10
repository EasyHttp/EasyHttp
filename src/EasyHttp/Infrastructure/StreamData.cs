using System.IO;

namespace EasyHttp.Infrastructure
{
    public class StreamData : MultiPartFileDataAbstraction
    {
        public Stream Stream { get; set; }
        public string FileNameForDisposition { get; set; }

        public override Stream GetStream()
        {
            return Stream;
        }

        public override long GetLength()
        {
            return Stream.Length;
        }

        public override string GetFilenameForDisposition()
        {
            return FileNameForDisposition;
        }
    }
}