using EasyHttp.Http;
using NUnit.Framework;

namespace EasyHttp.Specs.Specs
{
    public class when_making_a_GET_with_stream_response_true
    {
        HttpClient httpClient;

        [SetUp]
        public void SetUp()
        {
            httpClient = new HttpClient();

            httpClient.StreamResponse = true;
        }

        [Test(TestOf = typeof(HttpClient))]
        public void should_allow_access_to_response_stream()
        {
            httpClient.Get("http://localhost:16000/hello");

            using (var stream = httpClient.Response.ResponseStream)
            {
                int count;
                int total = 0;
                var buffer = new byte[8192];

                while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    total += count;
                }
                Assert.IsTrue(total > 0);
            }
        }

        [Test(TestOf = typeof(HttpClient))]
        public void raw_text_should_be_empty()
        {
            httpClient.Get("http://localhost:16000/hello");

            Assert.IsNull(httpClient.Response.RawText);
        }
    }
}