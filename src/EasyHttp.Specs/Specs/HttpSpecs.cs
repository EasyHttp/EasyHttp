using System;
using System.IO;
using EasyHttp.Http;
using EasyHttp.Specs.Helpers;
using Machine.Specifications;

namespace EasyHttp.Specs.Specs
{
    [Subject("HttpClient")]
    public class when_making_a_DELETE_request_with_a_valid_uri 
    {
        Establish context = () =>
        {
            httpClient = new HttpClient();
            httpClient.Request.Accept = HttpContentTypes.ApplicationJson;
            
            // First create customer in order to then delete it
            guid = Guid.NewGuid();

            httpClient.Put(string.Format("{0}/{1}", TestSettings.CouchDbDatabaseUrl, guid),
                          new Customer() {Name = "ToDelete", Email = "test@test.com"}, HttpContentTypes.ApplicationJson);

            response = httpClient.Response;

            rev = response.DynamicBody.rev;
        };

        Because of = () =>
        {

            httpClient.Delete(String.Format("{0}/{1}?rev={2}", TestSettings.CouchDbDatabaseUrl, guid, rev));
            response = httpClient.Response;
        };

        It should_delete_the_specified_resource = () =>
        {
            bool ok = response.DynamicBody.ok;

            string id = response.DynamicBody.id;

            ok.ShouldBeTrue();

            id.ShouldNotBeEmpty();

        };


        static HttpClient httpClient;
        static dynamic response;
        static string rev;
        static Guid guid;
    }

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

    [Subject("HttpClient")]
    public class when_making_a_HEAD_request_with_valid_uri
    {
        Establish context = () =>
        {
            httpClient = new HttpClient();
        };

        Because of = () =>
        {
            httpClient.Head(TestSettings.CouchDbRootUrl);
            httpResponse = httpClient.Response;

        };

        It should_return_correct_header =
            () => httpResponse.StatusDescription.ShouldEqual("OK");

        static HttpClient httpClient;
        static HttpResponse httpResponse;
    }

    [Subject("HttpClient")]
    public class when_creating_a_new_instance
    {
        Because of = () =>
        {
            httpClient = new HttpClient(); ;
        };

        It should_return_new_instance = () => httpClient.ShouldNotBeNull();

        static HttpClient httpClient;
    }

    [Subject("HttpClient")]
    public class when_making_a_POST_request_with_valid_uri_and_valid_data_and_content_type_set_to_application_json
    {
        Establish context = () =>
        {
            httpClient = new HttpClient();
            httpClient.Request.Accept = HttpContentTypes.ApplicationJson;

        };

        Because of = () =>
        {

            httpClient.Post(TestSettings.CouchDbDatabaseUrl, new Customer() { Name = "Hadi", Email = "test@test.com" }, HttpContentTypes.ApplicationJson);

            response = httpClient.Response;
        };


        It should_succeed = () =>
        {
            bool ok = response.DynamicBody.ok;

            string id = response.DynamicBody.id;

            ok.ShouldBeTrue();

            id.ShouldNotBeEmpty();
        };



        static HttpClient httpClient;
        static dynamic response;
    }

    [Subject("HttpClient")]
    public class when_making_a_PUT_request_with_valid_uri_and_valid_data_and_content_type_set_to_application_json
    {
        Establish context = () =>
        {
            httpClient = new HttpClient();
            httpClient.Request.Accept = HttpContentTypes.ApplicationJson;
        };

        Because of = () =>
        {
            Guid guid = Guid.NewGuid();
            httpClient.Put(string.Format("{0}/{1}", TestSettings.CouchDbDatabaseUrl, guid),
                          new Customer() { Name = "Put", Email = "test@test.com" }, HttpContentTypes.ApplicationJson);

            response = httpClient.Response;
        };


        It should_succeed = () =>
        {
            bool ok = response.DynamicBody.ok;

            string id = response.DynamicBody.id;

            ok.ShouldBeTrue();

            id.ShouldNotBeEmpty();
        };



        static HttpClient httpClient;
        static dynamic response;
    }
}