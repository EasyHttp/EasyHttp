namespace EasyHttp.Codecs
{
    public interface ICodec
    {
        byte[] Encode(object data, string contentType);
        T DecodeToStatic<T>(string rawText, string contentType);
        dynamic DecodeToDynamic(string rawText, string contentType);
    }
}