using Machine.Specifications;

namespace RestClient.Specs
{
    [Subject("RestClient")]
    public class when_creating_a_new_instance
    {
        Because of = () =>
        {
            restClient = new RestClient(); ;
        };

        It should_return_new_instance = () => restClient.ShouldNotBeNull();
        
        static RestClient restClient;
    }

    [Subject("RestClient")]
    public class when_requesting_valid_uri_in_text_format
    {
        Establish context = () =>
        {
            restClient = new RestClient();
        };

        Because of = () =>
        {
            httpResponse = restClient.Get("http://localhost:5984");

        };

        It should_return_body_with_rawtext =
            () => httpResponse.Body.RawText.ShouldEqual("{\"couchdb\":\"Welcome\",\"version\":\"1.0.0\"}\n");

        static RestClient restClient;
        static HttpResponse httpResponse;
    }

    [Subject("RestClient")]
    public class when_requesting_a_valid_uri_in_json_format
    {
        Establish context = () =>
        {
            restClient = new RestClient();
        };

        Because of = () =>
        {
            
            response = restClient.Get("http://localhost:5984");
        };


        It should_return_body_with_json_object = () =>
        {
            string couchdb = response.Body.couchdb;
      
            string version = response.Body.version;
          
            couchdb.ShouldEqual("Welcome");
            
            version.ShouldEqual("1.0.0");
        };

        static RestClient restClient;
        static dynamic response;
    }

   
}
