using System.IO;
using System.Reflection;
using EasyHttp.Http;
using NUnit.Framework;

namespace EasyHttp.Specs.Specs
{
    [TestFixture(TestOf = typeof(HttpClient))]
    public class FileRequestSpecs
    {
        [Test]
        public void when_making_a_GET_provided_filename_it_should_download_file_to_specified_filename()
        {
            var httpClient = new HttpClient();
            var filename = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "image.gif");
            File.Delete(filename);

            httpClient.GetAsFile("http://www.jetbrains.com/img/logos/logo_jetbrains.gif", filename);

            Assert.True(File.Exists(filename));
        }
    }
}