using System.Dynamic;
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
            httpClient = new HttpClient();
        };

        Because of = () =>
        {
            httpResponse = httpClient.Get("http://localhost:5984");

        };

        It should_return_body_with_rawtext =
            () => httpResponse.RawText.ShouldNotBeEmpty();

        static HttpClient httpClient;
        static HttpResponse httpResponse;
    }

    [Subject("HttpClient")]
    public class when_making_a_GET_request_with_valid_uri_and_content_type_set_to_application_json
    {
        Establish context = () =>
        {
            httpClient = new HttpClient();
            httpClient.Request.Accept = HttpContentTypes.ApplicationJson;

        };

        Because of = () =>
        {

            response = httpClient.Get(TestSettings.CouchDbRootUrl);
            
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

        static HttpClient httpClient;
        static HttpResponse response;
    }


    //[Subject("HttpClient")]
    //public class when_making_a_GET_request_with_valid_uri_and_content_type_set_to_application_xml
    //{
    //    Establish context = () =>
    //    {
    //        httpClient = new HttpClient();
    //        httpClient.Request.Accept = HttpContentTypes.ApplicationXml;

    //    };

    //    Because of = () =>
    //    {
    //        dynamic credentials = new ExpandoObject();

    //        credentials.login = "youtrackapi";
    //        credentials.password = "youtrackapi";

    //        httpClient.Post("http://youtrack.jetbrains.net/rest/user/login", credentials, HttpContentTypes.ApplicationXWwwFormUrlEncoded);

    //        response = httpClient.Response;
    //    };


    //    It should_return_dynamic_body_with_json_object = () =>
    //    {
    //        dynamic body = response.DynamicBody;

    //        string login = body.login;
            
    //        login.ShouldEqual("ok");
    //    };


    //    static HttpClient httpClient;
    //    static HttpResponse response;
    //}


}
