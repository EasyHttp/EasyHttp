using EasyHttp.Http;
using Machine.Specifications;

namespace EasyHttp.Specs.BugRepros
{
    public class when_adding_extra_header_to_request
    {
        Establish context = () =>
            {
                http = new HttpClient();

            };

        Because of = () =>
            {
                http.Request.AddExtraHeader("X-Header", "X-Value");
            };

        It should_add_it_to_the_request = () => { http.Request.RawHeaders.ContainsKey("X-Header"); };
        static HttpClient http;
    }
}