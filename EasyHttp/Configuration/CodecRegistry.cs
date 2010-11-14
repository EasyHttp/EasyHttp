using EasyHttp.Codecs;
using EasyHttp.Codecs.JsonFXExtensions;
using JsonFx.Json;
using JsonFx.Serialization;
using JsonFx.Serialization.Resolvers;
using JsonFx.Xml;
using StructureMap.Configuration.DSL;

namespace EasyHttp.Configuration
{
    public class CodecRegistry: Registry
    {
        public CodecRegistry()
        {
            For<ICodec>().Use<DefaultCodec>();
            For<IDataReader>().Singleton().Use<JsonReader>();
            For<IDataReader>().Singleton().Use<JsonReader>();
            For<IDataWriter>().Singleton().Use<JsonWriter>();
            For<IDataWriter>().Singleton().Use<XmlWriter>();
            For<IDataWriter>().Singleton().Use<UrlEncoderWriter>();
            For<IResolverStrategy>().Use<PocoResolverStrategy>();
        }
    }
}