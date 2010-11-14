using EasyHttp.Http;
using EasyHttp.Specs.Helpers;
using Machine.Specifications;

namespace EasyHttp.Specs.Specifications.Http
{


    [Subject("HttpClient")]
    public class when_making_a_GET_request_with_valid_uri
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

    [Subject("HttpClient")]
    public class when_making_a_GET_request_with_valid_uri_and_content_type_set_to_application_json
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
