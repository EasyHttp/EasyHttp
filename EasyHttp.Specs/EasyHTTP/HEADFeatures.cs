using Machine.Specifications;

namespace EasyHttp.Specs.EasyHTTP
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
            _httpClient.Head("http://localhost:5984");
            _httpResponse = _httpClient.Response;

        };

        It should_return_correct_header =
            () => _httpResponse.StatusDescription.ShouldEqual("OK");

        static HttpClient _httpClient;
        static HttpResponse _httpResponse;
    }

   
}
