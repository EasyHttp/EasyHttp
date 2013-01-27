using System.IO;
using System.Text;

namespace EasyHttp.Infrastructure
{
    public static class StreamExtensions
    {
        public static void WriteString(this Stream stream, string value)
        {
            var buffer = Encoding.ASCII.GetBytes(value);

            stream.Write(buffer, 0, buffer.Length);
        }
    }
}