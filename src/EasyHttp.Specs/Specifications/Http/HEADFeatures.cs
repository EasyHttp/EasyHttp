using EasyHttp.Http;
using EasyHttp.Specs.Helpers;
using Machine.Specifications;

namespace EasyHttp.Specs.Specifications.Http
{


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

   
}
