using EasyHttp.Specs.Helpers;
using Machine.Specifications;

namespace EasyHttp.Specs.Specifications.HttpMethods
{


    [Subject("Working with HEAD")]
    public class when_requesting_a_head_for_valid_uri
    {
        Establish context = () =>
        {
            _httpClient = new HttpClient();
        };

        Because of = () =>
        {
            _httpClient.Head(TestSettings.CouchDbRootUrl);
            _httpResponse = _httpClient.Response;

        };

        It should_return_correct_header =
            () => _httpResponse.StatusDescription.ShouldEqual("OK");

        static HttpClient _httpClient;
        static HttpResponse _httpResponse;
    }

   
}
