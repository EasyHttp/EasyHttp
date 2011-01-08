using System.IO;
using EasyHttp.Http;
using Machine.Specifications;

namespace EasyHttp.Specs.Specs
{
    [Subject(typeof(HttpClient))]
    public class when_requesting_a_get_as_file
    {
        Establish context = () =>
        {
            httpClient = new HttpClient();
            File.Delete(@"X:\Temp\banner.png");
        };

        Because of = () =>
        {
            httpClient.GetAsFile("http://www.jetbrains.com/img/banners/tc6_fb.png", @"X:\Temp\banner.png");
        };

        It should_save_the_request_to_a_file_to_specified_location = () =>
        {
            File.Exists(@"X:\Temp\banner.png").ShouldBeTrue();
        };

        static HttpClient httpClient;
    }

  
}