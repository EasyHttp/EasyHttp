using System;
using System.Collections.Generic;
using EasyHttp.Codecs;
using EasyHttp.Http;
using JsonFx.Json;
using JsonFx.Json.Resolvers;
using JsonFx.Serialization;
using JsonFx.Serialization.Resolvers;
using JsonFx.Xml.Resolvers;
using Machine.Specifications;

namespace EasyHttp.Specs.BugRepros
{
    [Subject("Custom Decoding")]
    public class when_decoding_an_object_with_custom_naming_of_property
    {

        Establish context = () =>
        {
            IEnumerable<IDataReader> readers = new List<IDataReader> { new JsonReader(new DataReaderSettings(new JsonResolverStrategy()), "application/.*json") };
            IEnumerable<IDataWriter> writers = new List<IDataWriter> { new JsonWriter(new DataWriterSettings(), "application/.*json") };

            codec = new DefaultCodec(new RegExBasedDataReaderProvider(readers), new RegExBasedDataWriterProvider(writers));
        };

        Because of = () =>
        {

            obj = codec.DecodeToStatic<CustomNaming>("{\"abc\":\"def\"}", "application/json");
        };

        It should_decode_taking_into_account_custom_property_name = () =>
        {
            obj.PropertyName.ShouldEqual("def");
        };

        static DefaultCodec codec;
        static CustomNaming obj;
    }

    
    [Subject("Custom Encoding")]
    public class when_encoding_an_object_with_custom_naming_of_property
    {
        static CombinedResolverStrategy CombinedResolverStrategy()
        {
            return new CombinedResolverStrategy(
                new JsonResolverStrategy(),
                new DataContractResolverStrategy(),
                new XmlResolverStrategy(),
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.PascalCase),
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.CamelCase),
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.Lowercase, "-"),
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.Uppercase, "_"));
        }

        Establish context = () =>
        {
    
            
            IEnumerable<IDataReader> readers = new List<IDataReader> { new JsonReader(new DataReaderSettings(), "application/.*json") };
            IEnumerable<IDataWriter> writers = new List<IDataWriter> { new JsonWriter(new DataWriterSettings(CombinedResolverStrategy()), HttpContentTypes.ApplicationJson) };

            codec = new DefaultCodec(new RegExBasedDataReaderProvider(readers), new RegExBasedDataWriterProvider(writers));
        };

        Because of = () =>
        {
            var customObject = new CustomNamedObject {UpperPropertyName = "someValue"};

            encoded = codec.Encode(customObject, HttpContentTypes.ApplicationJson);


        };

        It should_decode_taking_into_account_custom_property_name = () =>
        {
            var str = System.Text.Encoding.UTF8.GetString(encoded);
            str.ShouldContain("upperPropertyName");
        };

        static DefaultCodec codec;
        static byte[] encoded;
    }

    public class CustomNamedObject
    {
        [JsonName("upperPropertyName")]
        public string UpperPropertyName { get; set; }
    }


    public class CustomNaming
    {
        [JsonName("abc")]
        public string PropertyName { get; set; }
    }
 
}