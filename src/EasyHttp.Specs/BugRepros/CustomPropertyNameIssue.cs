using System.Collections.Generic;
using EasyHttp.Codecs;
using EasyHttp.Codecs.JsonFXExtensions;
using EasyHttp.Http;
using JsonFx.Json;
using JsonFx.Json.Resolvers;
using JsonFx.Serialization;
using JsonFx.Serialization.Resolvers;
using JsonFx.Xml.Resolvers;
using NUnit.Framework;

namespace EasyHttp.Specs.BugRepros
{
    public class CustomPropertyNameIssue
    {
        [Test, Category("Custom Decoding")]
        public void when_decoding_an_object_with_custom_naming_of_property_should_decode_taking_into_account_custom_property_name()
        {
            var combinedResolverStrategy = new CombinedResolverStrategy(
                new JsonResolverStrategy(),
                new DataContractResolverStrategy(),
                new XmlResolverStrategy(),
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.PascalCase),
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.CamelCase),
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.Lowercase, "-"),
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.Uppercase, "_"));

            IEnumerable<IDataReader> readers = new List<IDataReader>
            {
                new JsonReader(new DataReaderSettings(combinedResolverStrategy), HttpContentTypes.ApplicationJson)
            };

            var decoder = new DefaultDecoder(new RegExBasedDataReaderProvider(readers));

            var obj = decoder.DecodeToStatic<CustomNaming>("{\"abc\":\"def\"}", "application/json");

            Assert.AreEqual("def", obj.PropertyName);
        }

        [Test, Category("Custom Encoding")]
        public void when_encoding_an_object_with_custom_naming_of_property_it_should_decode_taking_into_account_custom_property_name()
        {
            var CombinedResolverStrategy = new CombinedResolverStrategy(
                new JsonResolverStrategy(),
                new DataContractResolverStrategy(),
                new XmlResolverStrategy(),
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.PascalCase),
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.CamelCase),
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.Lowercase, "-"),
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.Uppercase, "_"));

            IEnumerable<IDataWriter> writers = new List<IDataWriter>
            {
                new JsonWriter(new DataWriterSettings(CombinedResolverStrategy), HttpContentTypes.ApplicationJson)
            };

            var encoder = new DefaultEncoder(new RegExBasedDataWriterProvider(writers));

            var customObject = new CustomNamedObject {UpperPropertyName = "someValue"};

            var encoded = encoder.Encode(customObject, HttpContentTypes.ApplicationJson);

            var str = System.Text.Encoding.UTF8.GetString(encoded);

            StringAssert.Contains("upperPropertyName", str);
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
}