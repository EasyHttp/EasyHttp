using EasyHttp.Http;
using EasyHttp.Specs.Helpers;
using NUnit.Framework;

namespace EasyHttp.Specs.Specs
{
    [TestFixture(Category = "HttpClient Init")]
    public class when_creating_a_new_instance
    {
        [Test]
        public void when_creating_a_new_instance_it_should_return_new_instance_using_the_default_configuration()
        {
            var client = new HttpClient();

            Assert.NotNull(client);
        }

        [Test]
        public void when_creating_a_new_instance_with_base_url_should_prefix_all_requests_with_the_base_url()
        {
            var client = new HttpClient("http://localhost:16000");

            client.Get("/hello");

            Assert.AreEqual("http://localhost:16000/hello", client.Request.Uri);
        }
    }
}