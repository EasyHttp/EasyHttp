using EasyHttp.Http;
using EasyHttp.Specs.Helpers;
using Machine.Specifications;

namespace EasyHttp.Specs.Specs
{
    [Subject("HttpClient Init")]
    public class when_creating_a_new_instance
    {
        static HttpClient httpClient;
        Because of = () => { httpClient = new HttpClient(); };

        It should_return_new_instance_using_default_configuration = () => httpClient.ShouldNotBeNull();
    }

    [Subject("Initializing with base url")]
    public class when_creating_a_new_instance_with_base_url
    {
        Because of = () =>
        {
            httpClient = new HttpClient("http://localhost:16000");

            httpClient.Get("/hello");
        };

        It should_prefix_all_requests_with_the_base_url = () => httpClient.Request.Uri.ShouldEqual("http://localhost:16000/hello");

        static HttpClient httpClient;
    }
}