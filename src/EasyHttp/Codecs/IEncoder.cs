namespace EasyHttp.Codecs
{
    public interface IEncoder
    {
        byte[] Encode(object input, string contentType);
    }
}