using Machine.Specifications;

namespace EasyHttp.Specs.EasyHTTP
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
            () => _httpResponse.Body.RawText.ShouldEqual("{\"couchdb\":\"Welcome\",\"version\":\"1.0.0\"}\n");

        static HttpClient _httpClient;
        static HttpResponse _httpResponse;
    }

    [Subject("Working with GET")]
    public class when_requesting_a_valid_uri_in_json_format
    {
        Establish context = () =>
        {
            _httpClient = new HttpClient()
                            .WithAccept("application/json");

        };

        Because of = () =>
        {

            response = _httpClient.Get("http://127.0.0.1:5984/");
            
        };


        It should_return_body_with_json_object = () =>
        {
            string couchdb = response.Body.couchdb;
      
            string version = response.Body.version;
          
            couchdb.ShouldEqual("Welcome");
            
            version.ShouldEqual("1.0.0");
        };

        static HttpClient _httpClient;
        static dynamic response;
    }

}
