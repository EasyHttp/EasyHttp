using System;

namespace EasyHttp
{
    public interface ICoDec
    {
        byte[] Encode(object data, string contentType);
        T Decode<T>(string rawText, string contentType);
    }
}