using System.Collections.Generic;
using EasyHttp.Codecs;
using EasyHttp.Codecs.JsonFXExtensions;
using EasyHttp.Http;
using JsonFx.Json;
using JsonFx.Serialization;
using NUnit.Framework;

namespace EasyHttp.Specs.BugRepros
{
    public class EncodingEnums
    {
        [Test, Category("Encoding Enums")]
        public void when_encoding_an_object_that_contains_an_enum()
        {
            IEnumerable<IDataWriter> writers = new List<IDataWriter>
            {
                new JsonWriter(new DataWriterSettings(), "application/.*json")
            };

            var _encoder = new DefaultEncoder(new RegExBasedDataWriterProvider(writers));

            var data = new Foo {Baz = Bar.First};

            var result = _encoder.Encode(data, "application/vnd.fubar+json");

            Assert.Greater(result.Length, 0);
        }
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
