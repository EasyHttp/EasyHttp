using Machine.Specifications;

namespace EasyHttp.Specs.EasyHTTP
{


    [Subject("Working with GET")]
    public class when_requesting_valid_uri_in_text_format
    {
        Establish context = () =>
        {
            _easyHttp = new EasyHttp();
        };

        Because of = () =>
        {
            _easyHttp.Get("http://localhost:5984");
            response = _easyHttp.Response;

        };

        It should_return_body_with_rawtext =
            () => response.Body.RawText.ShouldEqual("{\"couchdb\":\"Welcome\",\"version\":\"1.0.0\"}\n");

        static EasyHttp _easyHttp;
        static Response response;
    }

    [Subject("Working with GET")]
    public class when_requesting_a_valid_uri_in_json_format
    {
        Establish context = () =>
        {
            _easyHttp = new EasyHttp();
            _easyHttp.SetAccept("application/json");
        };

        Because of = () =>
        {

            _easyHttp.Get("http://127.0.0.1:5984/");
            response = _easyHttp.Response;
        };


        It should_return_body_with_json_object = () =>
        {
            string couchdb = response.Body.couchdb;
      
            string version = response.Body.version;
          
            couchdb.ShouldEqual("Welcome");
            
            version.ShouldEqual("1.0.0");
        };

        static EasyHttp _easyHttp;
        static dynamic response;
    }


}
