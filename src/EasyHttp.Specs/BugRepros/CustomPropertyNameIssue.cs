using System;
using System.Collections.Generic;
using EasyHttp.Codecs;
using EasyHttp.Http;
using JsonFx.Json;
using JsonFx.Json.Resolvers;
using JsonFx.Serialization;
using Machine.Specifications;

namespace EasyHttp.Specs.BugRepros
{
    [Subject("Encoding Enums")]
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

    public class CustomNaming
    {
        [JsonName("abc")]
        public string PropertyName { get; set; }
    }
 
}