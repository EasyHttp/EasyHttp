using EasyHttp.Http;
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
            httpClient = new HttpClient("http://localhost:5984");

            httpClient.Get("/_all_dbs");
        };

        It should_prefix_all_requests_with_the_base_url = () => httpClient.Request.Uri.ShouldEqual("http://localhost:5984/_all_dbs");


        static HttpClient httpClient;
    }
}