using System.IO;
using System.Reflection;
using EasyHttp.Http;
using Machine.Specifications;

namespace EasyHttp.Specs.Specs
{
    [Subject(typeof(HttpClient))]
    public class when_making_a_GET_provided_filename
    {
        Establish context = () =>
        {
            httpClient = new HttpClient();

            filename = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "image.gif");

            File.Delete(filename);
        };

        Because of = () =>
        {
            httpClient.GetAsFile("http://www.jetbrains.com/img/logos/logo_jetbrains.gif",
                           filename);

        };

        It should_download_file_to_specified_filename = () =>
        {
            File.Exists(filename).ShouldBeTrue();
        };

        static HttpClient httpClient;
        static string filename;
    }

}