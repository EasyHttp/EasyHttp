using System.IO;
using System.Reflection;
using EasyHttp.Http;
using Machine.Specifications;

namespace EasyHttp.Specs.Specs
{
    [Subject(typeof(HttpClient))]
    public class when_making_a_GET_with_stream_response_true
    {
        Establish context = () =>
        {
            httpClient = new HttpClient();
        };

        Because of = () =>
        {
            httpClient.StreamResponse = true;

            httpClient.Get("http://localhost:16000/hello");
           
        };

        It should_allow_access_to_response_stream = () =>
        {
             using (var stream = httpClient.Response.ResponseStream)
             {
                int count;
                int total = 0;
                var buffer = new byte[8192];

                while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    total += count;
                }
                total.ShouldBeGreaterThan(0);
             }

        };

        It raw_text_should_be_empty = () =>
        {
            httpClient.Response.RawText.ShouldBeNull();
        };
        static HttpClient httpClient;
    }
}