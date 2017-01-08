using System.Net;
using EasyHttp.Http;
using NUnit.Framework;

namespace EasyHttp.Specs.Specs
{
    [TestFixture(Category = "HttpClient")]
    public class AutoFollowRedirectSpecs
    {
        public class when_making_a_GET_request_with_AutoRedirect_on
        {
            private static HttpClient httpClient;

            [SetUp]
            public void BecauseOf()
            {
                httpClient = new HttpClient();

                httpClient.Get("http://localhost:16000/redirector");
            }

            [Test]
            public void should_return_status_code_of_OK()
            {
                Assert.AreEqual(HttpStatusCode.OK, httpClient.Response.StatusCode);
            }

            [Test]
            public void should_redirect()
            {
                Assert.IsEmpty(httpClient.Response.Location);
            }
        }

        public class when_making_a_GET_request_with_AutoRedirect_off
        {
            HttpClient httpClient;

            [SetUp]
            public void BecauseOf()
            {
                httpClient = new HttpClient();
                httpClient.Request.AllowAutoRedirect = false;

                httpClient.Get("http://localhost:16000/redirector");
            }

            [Test]
            public void should_return_status_code_of_Redirect()
            {
                Assert.AreEqual(HttpStatusCode.Redirect, httpClient.Response.StatusCode);
            }

            [Test]
            public void should_redirect()
            {
                StringAssert.EndsWith("redirected", httpClient.Response.Location);
            }
        }
    }
}