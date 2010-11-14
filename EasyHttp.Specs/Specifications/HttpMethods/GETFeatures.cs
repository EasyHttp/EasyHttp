using EasyHttp.Http;
using EasyHttp.Specs.Helpers;
using Machine.Specifications;

namespace EasyHttp.Specs.Specifications.HttpMethods
{


    [Subject("Working with GET")]
    public class when_requesting_valid_uri_in_text_format
    {
        Establish context = () =>
        {
            _httpClient = new HttpClient();
        };

        Because of = () =>
        {
            _httpResponse = _httpClient.Get("http://localhost:5984");

        };

        It should_return_body_with_rawtext =
            () => _httpResponse.RawText.ShouldNotBeEmpty();

        static HttpClient _httpClient;
        static HttpResponse _httpResponse;
    }

    [Subject("Working with GET")]
    public class when_requesting_a_valid_uri_in_json_format
    {
        Establish context = () =>
        {
            _httpClient = new HttpClient();
            _httpClient.Request.Accept = HttpContentTypes.ApplicationJson;

        };

        Because of = () =>
        {

            response = _httpClient.Get(TestSettings.CouchDbRootUrl);
            
        };


        It should_return_dynamic_body_with_json_object = () =>
        {
            dynamic body = response.DynamicBody;

            string couchdb = body.couchdb;

            string version = body.version;
          
            couchdb.ShouldEqual("Welcome");
            
            version.ShouldNotBeEmpty();
        };

        static HttpClient _httpClient;
        static HttpResponse response;
    }

    [Subject("Working with GET")]
    public class when_requesting_a_valid_uri_in_json_format_for_static_body
    {
        Establish context = () =>
        {
            _httpClient = new HttpClient();
            _httpClient.Request.Accept = HttpContentTypes.ApplicationJson;

        };

        Because of = () =>
        {

            response = _httpClient.Get(TestSettings.CouchDbRootUrl);

        };


        It should_return_static_body_with_json_object = () =>
        {
            var couchInformation = response.StaticBody<CouchInformation>();

            couchInformation.couchdb.ShouldEqual("Welcome");

            couchInformation.version.ShouldNotBeEmpty();
        };

        static HttpClient _httpClient;
        static HttpResponse response;
    }

}
