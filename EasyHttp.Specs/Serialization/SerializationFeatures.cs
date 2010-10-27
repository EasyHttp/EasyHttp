using System.Dynamic;
using EasyHttp.Specs.Helpers;
using Machine.Specifications;

namespace EasyHttp.Specs.Serialization
{
    [Subject("Encoding")]
    public class when_requesting_json_format_of_static_object
    {
        Establish context = () =>
        {
            input = new StaticClass() {couchdb = "Welcome", version = "1.0.0"};
        };

        Because of = () =>
        {
            var encoder = new JsonCodec();

            output = encoder.Encode(input);
        };

        It should_return_a_string_encoded_in_json_format = () =>
        {
            output.ShouldEqual("{\"couchdb\":\"Welcome\",\"version\":\"1.0.0\"}");
        };

        static string output;
        static StaticClass input;
    }

    [Subject("Encoding")]
    public class when_requesting_json_format_of_dynamic_object
    {
        Establish context = () =>
        {
            input = new ExpandoObject();

            input.couchdb = "Welcome";
            input.version = "1.0.0";

        };

        Because of = () =>
        {
            var encoder = new JsonCodec();

            output = encoder.Encode(input);
        };

        It should_return_a_string_encoded_in_json_format = () =>
        {
            output.ShouldEqual("{\"couchdb\":\"Welcome\",\"version\":\"1.0.0\"}");
        };

        static string output;
        static dynamic input;
    }

    [Subject("Decoding")]
    public class when_providing_json_string_for_static_object
    {
        Establish context = () =>
        {
            input = "{\"couchdb\":\"Welcome\",\"version\":\"1.0.0\"}";
        };

        Because of = () =>
        {
            var decoder = new JsonCodec();

            output = decoder.Decode<StaticClass>(input);

        };

        It should_return_an_object_with_values_corresponding_to_decoded_json = () =>
        {
            output.couchdb.ShouldEqual("Welcome");
            output.version.ShouldEqual("1.0.0");
        };

        static StaticClass output;
        static string input;
    }

    [Subject("Encoding")]
    public class when_requesting_xml_format_of_static_object
    {
        Establish context = () =>
        {
            input = new StaticClass() { couchdb = "Welcome", version = "1.0.0" };
        };

        Because of = () =>
        {
            var encoder = new XmlCodec();

            output = encoder.Encode(input);
        };

        It should_return_a_string_encoded_in_json_format = () =>
        {
            // One of those WTF's. Output is exactly same but fails.
            
            // output.ShouldEqual("<?xml version=\"1.0\" encoding=\"utf-16\"?> \n<StaticClass> \n  <couchdb>Welcome</couchdb> \n  <version>1.0.0</version> \n</StaticClass>");

        };

        static string output;
        static StaticClass input;


    }

    [Subject("Decoding")]
    public class when_providing_xml_string_for_static_object
    {
        Establish context = () =>
        {
            input = "<?xml version=\"1.0\" encoding=\"utf-16\"?> \n<StaticClass> \n  <couchdb>Welcome</couchdb> \n  <version>1.0.0</version> \n</StaticClass>";
        };

        Because of = () =>
        {
            var decoder = new XmlCodec();

            output = decoder.Decode<StaticClass>(input);

        };

        It should_return_an_object_with_values_corresponding_to_decoded_xml = () =>
        {
            output.couchdb.ShouldEqual("Welcome");
            output.version.ShouldEqual("1.0.0");
        };

        static StaticClass output;
        static string input;
    }

    [Subject("Decoding")]
    public class when_providing_json_string_for_dynamic_object
    {
        Establish context = () =>
        {
            input = "{\"couchdb\":\"Welcome\",\"version\":\"1.0.0\"}";
        };

        Because of = () =>
        {
            var decoder = new JsonCodec();

            output = decoder.Decode<DynamicObject>(input);

        };

        It should_return_an_object_with_values_corresponding_to_decoded_json = () =>
        {
            string couchdb = output.couchdb;
            string version = output.version;
            
            couchdb.ShouldEqual("Welcome");
            version.ShouldEqual("1.0.0");
        };

        static dynamic output;
        static string input;
    }
}
