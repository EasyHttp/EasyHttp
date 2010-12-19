using System;
using System.Collections.Generic;
using EasyHttp.Codecs;
using EasyHttp.Codecs.JsonFXExtensions;
using JsonFx.Json;
using JsonFx.Json.Resolvers;
using JsonFx.Serialization;
using JsonFx.Serialization.Resolvers;
using JsonFx.Xml;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace EasyHttp.Configuration
{
    public class ContainerConfiguration: Registry
    {
        public ContainerConfiguration()
        {
            For<ICodec>().Use<DefaultCodec>();
            For<IDataReader>().Singleton().Use<JsonReader>();
            For<IDataReader>().Singleton().Use<XmlReader>();
            For<IDataWriter>().Singleton().Use<JsonWriter>();
            For<IDataWriter>().Singleton().Use<XmlWriter>();
            For<IDataWriter>().Singleton().Use<UrlEncoderWriter>();
            For<IResolverStrategy>().Use<JsonResolverStrategy>();

            Scan(x => { x.AssembliesFromApplicationBaseDirectory();
                          x.AddAllTypesOf<IConfigurationStep>();
            });
        }
    }

    // TODO: Remove this. It's for testing right now....

    public class AddCustomContentTypeStep: IConfigurationStep
    {
        public void Execute(Registry registry)
        {
            registry.For<IDataReader>().Singleton().Use<CustomContentType>();

        }
    }

    public class CustomContentType: JsonReader
    {
        public override IEnumerable<string> ContentType
        {
            get { yield return "application/vnd.graemef+json"; }
        }
    }
}