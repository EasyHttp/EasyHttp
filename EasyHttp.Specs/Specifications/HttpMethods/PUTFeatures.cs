using System;
using EasyHttp.Specs.Helpers;
using Machine.Specifications;

namespace EasyHttp.Specs.Specifications.HttpMethods
{
    [Subject("Working with PUT")]
    public class when_putting_a_request_in_json_format
    {
        Establish context = () =>
        {
            _httpClient = new HttpClient()
                .WithAccept(HttpContentTypes.ApplicationJson);
        };

        Because of = () =>
        {
            Guid guid = Guid.NewGuid();
            _httpClient.Put(string.Format("{0}/{1}", TestSettings.CouchDbDatabaseUrl, guid),
                          new Customer() {Name = "Put", Email = "test@test.com"}, HttpContentTypes.ApplicationJson);

            response = _httpClient.Response;
        };


        It should_succeed = () =>
        {
            bool ok = response.DynamicBody.ok;

            string id = response.DynamicBody.id;

            ok.ShouldBeTrue();

            id.ShouldNotBeEmpty();
        };



        static HttpClient _httpClient;
        static dynamic response;
    }
}