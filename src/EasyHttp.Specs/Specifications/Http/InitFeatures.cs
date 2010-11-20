using EasyHttp.Http;
using Machine.Specifications;

namespace EasyHttp.Specs.Specifications.Http
{
    [Subject("HttpClient")]
    public class when_creating_a_new_instance
    {
        Because of = () =>
        {
            httpClient = new HttpClient(); ;
        };

        It should_return_new_instance = () => httpClient.ShouldNotBeNull();

        static HttpClient httpClient;
    }
}