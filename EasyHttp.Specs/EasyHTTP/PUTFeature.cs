using System;
using EasyHttp.Specs.Helpers;
using Machine.Specifications;

namespace EasyHttp.Specs.EasyHTTP
{
    [Subject("Working with PUT")]
    public class when_putting_a_request_in_json_format
    {
        Establish context = () =>
        {
            _httpClient = new HttpClient()
                .WithAccept("application/json");
        };

        Because of = () =>
        {
            Guid guid = Guid.NewGuid();
            _httpClient.Put(string.Format("{0}/{1}", "http://127.0.0.1:5984/customers", guid),
                          new Customer() {Name = "Put", Email = "test@test.com"}, "application/json");

            response = _httpClient.Response;
        };


        It should_succeed = () =>
        {
            bool ok = response.Body.ok;

            string id = response.Body.id;

            ok.ShouldBeTrue();

            id.ShouldNotBeEmpty();
        };



        static HttpClient _httpClient;
        static dynamic response;
    }
}