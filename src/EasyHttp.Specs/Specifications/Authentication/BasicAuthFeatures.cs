using EasyHttp.Http;
using Machine.Specifications;

namespace EasyHttp.Specs.Specifications.Authentication
{
    [Ignore()]
    [Subject("Authentication")]
    public class when_performing_a_get_that_requires_authentication_with_valid_data
    {
        Establish context = () =>
        {
            _httpClient = new HttpClient();
        };

        Because of = () =>
        {
            _httpClient.Request.SetBasicAuthentication("iis_test_user", "logitech100!!!");
            _httpResponse = _httpClient.Get("http://localhost/testsite");

        };

        It should_process_the_request_correctly = () =>
        {
            
        };

        static HttpClient _httpClient;
        static HttpResponse _httpResponse;
    }

  
}
