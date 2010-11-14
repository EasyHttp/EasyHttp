using EasyHttp.Http;
using EasyHttp.Specs.Helpers;
using Machine.Specifications;

namespace EasyHttp.Specs.Specifications.HttpMethods
{
    [Subject("Working with POST")]
    public class when_posting_a_request_in_json_format
    {
        Establish context = () =>
        {
            _httpClient = new HttpClient();
            _httpClient.Request.Accept = HttpContentTypes.ApplicationJson;

        };

        Because of = () =>
        {

            _httpClient.Post(TestSettings.CouchDbDatabaseUrl, new Customer() { Name = "Hadi", Email = "test@test.com" }, HttpContentTypes.ApplicationJson);

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