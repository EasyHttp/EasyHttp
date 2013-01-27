using EasyHttp.Codecs;

namespace EasyHttp.Configuration
{
    public interface IEncoderDecoderConfiguration
    {
        IEncoder GetEncoder();
        IDecoder GetDecoder();
    }
}