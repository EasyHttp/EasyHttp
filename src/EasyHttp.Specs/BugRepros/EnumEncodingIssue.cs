using System.Collections.Generic;
using EasyHttp.Codecs;
using EasyHttp.Codecs.JsonFXExtensions;
using EasyHttp.Http;
using JsonFx.Json;
using JsonFx.Serialization;
using Machine.Specifications;




namespace EasyHttp.Specs.BugRepros
{
    [Subject("Encoding Enums")]
    public class when_encoding_an_object_that_contains_an_enum
    {

        Establish context = () =>
        {
            IEnumerable<IDataWriter> writers = new List<IDataWriter> { new JsonWriter(new DataWriterSettings(), "application/.*json") };

            _encoder = new DefaultEncoder(new RegExBasedDataWriterProvider(writers));
        };

        Because of = () =>
        {
            var data = new Foo {Baz = Bar.First};

            result = _encoder.Encode(data, "application/vnd.fubar+json");
        };

        It should_encode_correctly = () =>
        {
            result.Length.ShouldBeGreaterThan(0);
        };

        static HttpClient client;
        static DefaultEncoder _encoder;
        static byte[] result;
    }
 
    public class Foo
    {
        public Bar Baz { get; set; }
    }

    public enum Bar
    {
        First,
        Second,
        Third
    }
 
}
