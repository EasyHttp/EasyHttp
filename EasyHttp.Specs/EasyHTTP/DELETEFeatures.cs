using System;
using EasyHttp.Specs.Helpers;
using Machine.Specifications;

namespace EasyHttp.Specs.EasyHTTP
{
    [Subject("Working with DELETE")]
    public class when_deleting_a_resource 
    {
        Establish context = () =>
        {
            _httpClient = new HttpClient()
                .WithAccept("application/json");

            // First create customer in order to then delete it
            guid = Guid.NewGuid();

            _httpClient.Put(string.Format("{0}/{1}", TestSettings.CouchDbDatabaseUrl, guid),
                          new Customer() {Name = "ToDelete", Email = "test@test.com"}, "application/json");

            response = _httpClient.Response;

            rev = response.DynamicBody.rev;
        };

        Because of = () =>
        {

            _httpClient.Delete(String.Format("{0}/{1}?rev={2}", TestSettings.CouchDbDatabaseUrl, guid, rev));
            response = _httpClient.Response;
        };

        It should_delete_the_specified_resource = () =>
        {
            bool ok = response.DynamicBody.ok;

            string id = response.DynamicBody.id;

            ok.ShouldBeTrue();

            id.ShouldNotBeEmpty();

        };


        static HttpClient _httpClient;
        static dynamic response;
        static string rev;
        static Guid guid;
    }
}